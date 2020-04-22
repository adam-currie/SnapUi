using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.VisualTree;
using System.Diagnostics;
using System.Linq;

namespace SnapUi {

    public class DragOp : IDragOp {
        private readonly IDraggable draggable;
        private readonly DraggablePreview floatingPreviewImg;

        private IDropZone? candidateDropZone;

        public bool IsDisposed {get; private set;}

        public DragOp(IDraggable draggable, Point startingPoint) {
            this.draggable = draggable;

            floatingPreviewImg = new DraggablePreview(draggable);

            draggable.GetVisualAncestor<ISnapUiRoot>()
                .OverlayLayer
                .Children
                .Add(floatingPreviewImg);

            //calling update to get some state correct before first draw
            Update(startingPoint);
        }

        public void Update(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            ISnapUiRoot root = draggable.GetVisualAncestor<ISnapUiRoot>();

            /* todo: check if root has changed and migrate everything
             * -candidate dropzone may no longer be valid     
             * -preview needs to be reparented
             */

            UpdateFloatingPreview(point);
            UpdateCandidateDropZone(point, root);
        }

        private void UpdateCandidateDropZone(Point point, ISnapUiRoot root) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            Point rootPoint = (Point)draggable.TranslatePoint(point, root.VisualRoot)!;

            IDropZone? candidateCandidateDropZone = root.VisualRoot.Renderer
                .HitTest(rootPoint, root, (x) => true)
                .Where(IsValidDropZoneOrCurrentParent)
                .FirstOrDefault()
                as IDropZone;

            if (candidateCandidateDropZone == draggable.VisualParent) {
                //we don't want to trigger any switching logic!
                candidateCandidateDropZone = null;
            }

            candidateDropZone = candidateCandidateDropZone;
        }

        private void UpdateFloatingPreview(Point point) {
            Point previewPoint =
                (Point)((IVisual)draggable).TranslatePoint(point, floatingPreviewImg.Parent)!;
            floatingPreviewImg.RenderTransform =
                new MatrixTransform(Matrix.CreateTranslation(previewPoint));
        }

        private bool IsValidDropZoneOrCurrentParent(IVisual v) =>
            v == draggable.VisualParent ||
            !draggable.IsVisualAncestorOf(v) &&
            ((v as IDropZone)?.CanAdd(draggable) ?? false);

        /// <inheritdoc/>
        public void Release(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            if (candidateDropZone != null) {
                Debug.Assert(candidateDropZone != draggable.Parent);

                draggable.DropZoneParent.Remove(draggable);
                candidateDropZone.Add(draggable);
                candidateDropZone = null;
            }

            draggable.InvalidateVisual();

            Dispose();
        }

        public void Dispose() {
            if (!IsDisposed) {
                OverlayLayer overlay = (OverlayLayer)floatingPreviewImg.Parent;
                overlay.Children.Remove(floatingPreviewImg);
                IsDisposed = true;
            }
        }
    }
}

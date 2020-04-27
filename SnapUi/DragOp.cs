using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using SnapUi.Controls;
using System.Diagnostics;
using System.Linq;

namespace SnapUi {

    public class DragOp : IDragOp {
        private readonly IDraggable draggable;
        private readonly DragPreviewer floatingPreviewImg;
        private readonly DragPreviewer dropPreviewImg;

        private IDropZone? __candidateDropZone;
        private IDropZone? CandidateDropZone {
            get => __candidateDropZone;
            set {
                if (value != __candidateDropZone) {
                    __candidateDropZone?.RemovePreview(dropPreviewImg);
                    value?.AddPreview(dropPreviewImg);
                    __candidateDropZone = value;
                }
            }
        }

        public bool IsDisposed { get; private set; }

        public DragOp(IDraggable draggable, Point startingPoint) {
            this.draggable = draggable;

            floatingPreviewImg = new DragPreviewer(draggable, true);
            dropPreviewImg = new DragPreviewer(draggable, false);

            var children = draggable.GetVisualAncestor<ISnapUiRoot>()
                .OverlayLayer.Children;
            children.Add(floatingPreviewImg);


            //calling update to get some state correct before first draw
            Update(startingPoint);
        }

        public void Update(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            ISnapUiRoot root = draggable.GetVisualAncestor<ISnapUiRoot>();

            if (root.OverlayLayer != floatingPreviewImg.Parent) {
                /*
                 * Probably some smart alec messing with the hierarchy.
                 * We don't need to worry about this for candidate dropzone 
                 * since that updates with the new root anyway.
                 */
                ((OverlayLayer)floatingPreviewImg.Parent).Children.Remove(floatingPreviewImg);
                root.OverlayLayer.Children.Add(floatingPreviewImg);
            }

            UpdateFloatingPreviewPosition(point);
            UpdateCandidateDropZone(point, root);
        }

        private void UpdateCandidateDropZone(Point point, ISnapUiRoot root) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            Point rootPoint = (Point)draggable.TranslatePoint(point, root.VisualRoot)!;

            IDropZone? candidateCandidateDropZone = root.VisualRoot.Renderer
                .HitTest(rootPoint, root, (x) => true)
                .Where(IsValidDropZone)
                .FirstOrDefault()
                as IDropZone;

            CandidateDropZone = candidateCandidateDropZone;
        }

        private void UpdateFloatingPreviewPosition(Point point) {
            Point previewPoint =
                (Point)draggable.TranslatePoint(point, floatingPreviewImg.GetVisualParent())!;
            floatingPreviewImg.RenderTransform =
                new MatrixTransform(Matrix.CreateTranslation(previewPoint));
        }

        private bool IsValidDropZone(IVisual v) =>
            (v is IDropZone dropZone) &&
            !dropZone.LogicalChildren.Contains(draggable) && //can't go where we already are
            !draggable.IsVisualAncestorOf(dropZone) &&      //this would be like being your own grandfather
            dropZone.CanAddDraggable(draggable);

        /// <inheritdoc/>
        public void Release(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            if (CandidateDropZone != null) {
                Debug.Assert(!CandidateDropZone.GetLogicalChildren().Contains(draggable));

                /*
                 * removing draggable from old dropzone should cause this dragop 
                 * to be disposed so we need to cache state here
                 */
                IDropZone candidate = CandidateDropZone;

                draggable.DropZoneParent.RemoveDraggable(draggable);
                candidate.AddDraggable(draggable);
            }

            Dispose();
        }

        public void Dispose() {
            if (!IsDisposed) {
                OverlayLayer overlay = (OverlayLayer)floatingPreviewImg.Parent;
                overlay.Children.Remove(floatingPreviewImg);
                CandidateDropZone = null;//property will do its own cleanup
                IsDisposed = true;
            }
        }
    }
}

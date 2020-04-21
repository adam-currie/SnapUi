using Avalonia;
using Avalonia.VisualTree;
using System.Diagnostics;
using System.Linq;

namespace SnapUi {

    public class DragOp {
        private readonly IDraggable draggable;

        private IDropZone? candidateDropZone;

        //todo: when draggable is invalidated, redraw preview, otherwise use cached bitmap

        public DragOp(IDraggable draggable, Point startingPoint) {
            this.draggable = draggable;
        }

        public void Update(Point point) {
            ISnapUiRoot top = draggable.GetVisualAncestor<ISnapUiRoot>();
            Point screenPoint = (Point)draggable.TranslatePoint(point, top.VisualRoot)!;

            IDropZone? candidateCandidateDropZone = top.VisualRoot.Renderer
                .HitTest(screenPoint, top, (x) => true)
                .Where(IsValidDropZoneOrCurrentParent)
                .FirstOrDefault()
                as IDropZone;

            if (candidateCandidateDropZone == draggable.VisualParent) {
                //we don't want to trigger any switching logic!
                candidateCandidateDropZone = null;
            }

            //todo: this is probably redundant 
            if (candidateDropZone != candidateCandidateDropZone) {
                candidateDropZone = candidateCandidateDropZone;
            }

            draggable.InvalidateVisual();
        }

        private bool IsValidDropZoneOrCurrentParent(IVisual v) =>
            v == draggable.VisualParent ||
            !draggable.IsVisualAncestorOf(v) &&
            ((v as IDropZone)?.CanAdd(draggable) ?? false);

        public void Release(Point point) {
            if (candidateDropZone != null) {
                Debug.Assert(candidateDropZone != draggable.Parent);
                candidateDropZone.Add(draggable);
                candidateDropZone = null;
            }

            draggable.InvalidateVisual();
        }

    }
}

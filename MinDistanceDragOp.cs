using Avalonia;

namespace SnapUi {

    /// <summary>
    /// Triggers a real drag op only after dragging a minimum distance.
    /// </summary>
    public class MinDistanceDragOp {
        private readonly IDraggable draggable;
        private readonly Point startingPoint;

        DragOp? dragOp = null;

        public MinDistanceDragOp(IDraggable draggable, Point point) {
            this.draggable = draggable;
            this.startingPoint = point;
        }

        public void Update(Point point) {
            if (dragOp != null) {
                dragOp.Update(point);
            } else {
                Point dif = point - startingPoint;
                double distanceSquared = (dif.X * dif.X) + (dif.Y * dif.Y);

                int minDist = draggable.MinDragDistance;
                if(distanceSquared >= minDist*minDist) {
                    dragOp = new DragOp(draggable, point);
                }
            }
        }

        public void Release(Point point) => dragOp?.Release(point);

    }
}
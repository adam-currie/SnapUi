using Avalonia;
using SnapUi.Controls;
using SnapUi.DrapOps;

namespace SnapUi.DragOps {

    /// <summary>
    /// Triggers a real drag op only after dragging a minimum distance.
    /// </summary>
    public class MinDistanceDragOp : DragOpDecorator {
        private readonly IDraggable draggable;
        private readonly Point startingPoint;
        private readonly int minDistSquared;
        private readonly IDragOp.Create dragFactory;

        public MinDistanceDragOp(IDraggable draggable, Point point, int minDragDistance, IDragOp.Create dragFactory) 
            : base(null) {
            this.draggable = draggable;
            this.startingPoint = point;
            this.dragFactory = dragFactory;
            minDistSquared = minDragDistance * minDragDistance;
        }

        public override void Update(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            if (subDragOp != null) {
                subDragOp.Update(point);
            } else {
                Point dif = point - startingPoint;
                double distanceSquared = (dif.X * dif.X) + (dif.Y * dif.Y);

                if (distanceSquared >= minDistSquared) {
                    subDragOp = dragFactory(draggable, point);
                }
            }
        }

    }
}
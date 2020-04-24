using Avalonia;
using SnapUi.Controls;

namespace SnapUi {

    /// <summary>
    /// Triggers a real drag op only after dragging a minimum distance.
    /// </summary>
    public class MinDistanceDragOp : IDragOp {
        private readonly IDraggable draggable;
        private readonly Point startingPoint;
        private readonly int minDistSquared;
        private readonly IDragOp.Factory dragFactory;

        IDragOp? subDragOp = null;

        private bool _isDisposed = false;
        public bool IsDisposed {
            get {
                if (_isDisposed) {
                    return true;
                } else if (subDragOp?.IsDisposed ?? false) {
                    return _isDisposed = true;
                } else {
                    return false;
                }
            }
        }

        public MinDistanceDragOp(IDraggable draggable, Point point, int minDragDistance, IDragOp.Factory dragFactory) {
            this.draggable = draggable;
            this.startingPoint = point;
            this.dragFactory = dragFactory;
            minDistSquared = minDragDistance * minDragDistance;
        }

        public void Update(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            if (subDragOp != null) {
                subDragOp.Update(point);
            } else {
                Point dif = point - startingPoint;
                double distanceSquared = (dif.X * dif.X) + (dif.Y * dif.Y);

                if(distanceSquared >= minDistSquared) {
                    subDragOp = dragFactory(draggable, point);
                }
            }
        }

        /// <inheritdoc/>
        public void Release(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            subDragOp?.Release(point);
        }

        public void Dispose() {
            if (!IsDisposed) {
                if (subDragOp != null && !(subDragOp.IsDisposed)) {
                    subDragOp.Dispose();
                }
                _isDisposed = true;
            }
        }

    }
}
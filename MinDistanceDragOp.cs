using Avalonia;

namespace SnapUi {

    /// <summary>
    /// Triggers a real drag op only after dragging a minimum distance.
    /// </summary>
    public class MinDistanceDragOp : IDragOp {
        private readonly IDraggable draggable;
        private readonly Point startingPoint;

        //todo: maybe dependency injection?
        IDragOp? dragOp = null;

        private bool _isDisposed = false;
        public bool IsDisposed {
            get {
                if (_isDisposed) {
                    return true;
                } else if (dragOp?.IsDisposed ?? false) {
                    return _isDisposed = true;
                } else {
                    return false;
                }
            }
        }

        public MinDistanceDragOp(IDraggable draggable, Point point) {
            this.draggable = draggable;
            this.startingPoint = point;
        }

        public void Update(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

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

        /// <inheritdoc/>
        public void Release(Point point) {
            if (IsDisposed) {
                throw new System.ObjectDisposedException(ToString());
            }

            dragOp?.Release(point);
        }

        public void Dispose() {
            if (!IsDisposed) {
                if (dragOp != null && !(dragOp.IsDisposed)) {
                    dragOp.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
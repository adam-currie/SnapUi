using Avalonia;
using SnapUi.Controls;
using SnapUi.DragOps;
using System;

namespace SnapUi.DrapOps {


    /// <summary>
    /// Decorates an <see cref="IDragOp"/>.
    /// </summary>
    public abstract class DragOpDecorator : IDragOp {
        protected IDragOp? subDragOp;

        private bool _lazyIsDisposed = false;
        virtual public bool IsDisposed =>
            (_lazyIsDisposed || subDragOp == null) ?
                _lazyIsDisposed :
                _lazyIsDisposed = subDragOp.IsDisposed;

        virtual public IDropZone? CandidateDropZone
            => subDragOp?.CandidateDropZone ?? null;

        /// <param name="subDragOp">
        /// The DragOp to decorate. 
        /// This can be null since subclasses might instantiate it conditionally.
        /// </param>
        public DragOpDecorator(IDragOp? subDragOp) {

            this.subDragOp = subDragOp;
        }

        virtual public void Update(Point point) {
            if (IsDisposed) {
                throw new ObjectDisposedException(ToString());
            }

            subDragOp?.Update(point);
        }

        /// <inheritdoc/>
        virtual public void Release(Point point) {
            if (IsDisposed) {
                throw new ObjectDisposedException(ToString());
            }

            subDragOp?.Release(point);
        }

        virtual public void Dispose() {
            if (!IsDisposed) {
                if (subDragOp != null && !(subDragOp.IsDisposed)) {
                    subDragOp.Dispose();
                }
                _lazyIsDisposed = true;
            }
        }

    }
}

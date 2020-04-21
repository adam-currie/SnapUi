using Avalonia;
using System.Reactive.Disposables;

namespace SnapUi {
    public interface IDragOp : ICancelable {
        public void Update(Point point);

        /// <summary>
        /// Releases the drag operation, allowing a potential drop, this also calls <c cref="SnapUi.DragOp.Dispose()"/>
        /// </summary>
        /// <param name="point">The point.</param>
        public void Release(Point point);

    }
}

using Avalonia;
using SnapUi.Controls;
using System.Reactive.Disposables;

namespace SnapUi {
    public interface IDragOp : ICancelable {
        public void Update(Point point);

        /// <summary>
        /// Releases the drag operation, allowing a potential drop.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="SnapUi.DragOp.Dispose()"/>.
        /// </remarks>
        /// <param name="point">The point.</param>
        public void Release(Point point);

        public delegate IDragOp Factory(IDraggable draggable, Point startingPoint);

    }
}

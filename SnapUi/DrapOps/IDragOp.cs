using Avalonia;
using SnapUi.Controls;
using System;
using System.Reactive.Disposables;

namespace SnapUi.DragOps {
    public interface IDragOp : ICancelable {

        public IDropZone? CandidateDropZone { get; }

        public void Update(Point point);

        /// <summary>
        /// Releases the drag operation, allowing a potential drop.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="SnapUi.DragOp.Dispose()"/>.
        /// </remarks>
        /// <param name="point">The point.</param>
        public void Release(Point point);

        public delegate IDragOp Create(IDraggable draggable, Point startingPoint);

    }
}

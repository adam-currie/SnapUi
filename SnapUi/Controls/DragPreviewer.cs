using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SnapUi.Controls {
    internal class DragPreviewer : RemoteViewer {

        /// <summary>
        /// Indicates whether this is a floating preview or a drop preview.
        /// </summary>
        public bool IsFloating { get; }

        internal DragPreviewer(IRemoteViewingTarget target, bool isFloating)
            : base(target) => IsFloating = isFloating;

    }
}
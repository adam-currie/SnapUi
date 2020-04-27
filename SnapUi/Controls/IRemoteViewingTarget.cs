using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using System.Diagnostics;

namespace SnapUi.Controls {
    public interface IRemoteViewingTarget: IControl, ILayoutable {
        public event System.EventHandler MeasureInvalidated;

        public Size RemoteRenderMeasureOverride(Size remoteAvailableSize);

        /// <summary>
        /// Renders a remote view of the control.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="context">The context to render to.</param>
        /// <param name="temporaryArrangeAvailableSize">
        /// Used to temporarily re-arrange the Control.
        /// Use this with <see cref="Layoutable.ArrangeCore(Rect)"/>.
        /// </param>
        public void RemoteRender(RemoteViewer host, DrawingContext context, Rect temporaryArrangeAvailableSize);
    }
}

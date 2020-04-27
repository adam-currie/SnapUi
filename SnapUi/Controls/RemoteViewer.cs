using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SnapUi.Controls {

    /// <summary>
    /// Spooky control that mimics the view of another control in real-time.
    /// </summary>
    public class RemoteViewer : Control {

        public IRemoteViewingTarget Target { get; }

        internal RemoteViewer(IRemoteViewingTarget target)
            => Target = target;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);
            Target.MeasureInvalidated += DraggableMeasureInvalidated;
            InvalidateMeasure();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnDetachedFromVisualTree(e);
            Target.MeasureInvalidated -= DraggableMeasureInvalidated;
        }

        private void DraggableMeasureInvalidated(object? sender, System.EventArgs e) {
            InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize) =>
            (Target.IsVisible) ?
                Target.RemoteRenderMeasureOverride(availableSize) :
                new Size();

        public override void Render(DrawingContext context) {
            if (Target.IsVisible) {
                Target.RemoteRender(this, context, (Rect)((ILayoutable)this).PreviousArrange!);
            }
        }
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;

namespace SnapUi.Controls {
    public class SnapUiRoot : Decorator, ISnapUiRoot {

        public OverlayLayer OverlayLayer { get; }

        public SnapUiRoot() {
            OverlayLayer = new OverlayLayer();
            ((ISetLogicalParent)OverlayLayer).SetParent((ILogical)this);
            OverlayLayer.ZIndex = int.MaxValue;
            VisualChildren.Add(OverlayLayer);
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnAttachedToLogicalTree(e);
            ((ILogical)OverlayLayer).NotifyAttachedToLogicalTree(e);
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnDetachedFromLogicalTree(e);
            ((ILogical)OverlayLayer).NotifyDetachedFromLogicalTree(e);
        }

        protected override Size MeasureOverride(Size availableSize) {
            OverlayLayer.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            OverlayLayer.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

    }
}

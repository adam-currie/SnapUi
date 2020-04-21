using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;

namespace SnapUi {
    public class SnapUiRoot : Decorator, ISnapUiRoot {
        
        public OverlayLayer OverlayLayer { get; }

        public SnapUiRoot() {
            OverlayLayer = new OverlayLayer();

            //todo: check if we need to move this to OnInitialized or something
            ((ISetLogicalParent)OverlayLayer).SetParent((ILogical)this);
            OverlayLayer.ZIndex = int.MaxValue;
            VisualChildren.Add(OverlayLayer);
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnAttachedToLogicalTree(e);

            //todo: see if we need this?
            ((ILogical)OverlayLayer).NotifyAttachedToLogicalTree(e);
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e) {
            base.OnDetachedFromLogicalTree(e);

            //todo: see if we need this?
            ((ILogical)OverlayLayer).NotifyDetachedFromLogicalTree(e);
        }

    }
}

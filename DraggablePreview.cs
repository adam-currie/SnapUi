using Avalonia.Controls;
using Avalonia.Media;

namespace SnapUi {
    internal class DraggablePreview : Control {
        private IDraggable draggable;

        internal DraggablePreview(IDraggable draggable) {
            this.draggable = draggable;
            InvalidateVisual();
        }

        public override void Render(DrawingContext context) {
            base.Render(context);

            //todo: handle resizes?

            draggable.RenderPreview(context);

            //todo: see if we can listen for invalidation of the target so we don't have to constantly invalidate
            InvalidateVisual();
        }

    }
}
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi {
    public class DraggableBorder : Border, IDraggable {
        private readonly IDraggable.DragImplementor impl;

        static DraggableBorder() {
            //make this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DraggableBorder>(Brushes.Transparent);
        }


        public DraggableBorder() {
            impl = new IDraggable.DragImplementor(this);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e) {
            base.OnPointerPressed(e);
            impl.DraggablePointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e) {
            base.OnPointerMoved(e);
            impl.DraggablePointerMoved(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e) {
            base.OnPointerReleased(e);
            impl.DraggablePointerReleased(e);
        }

        public void RenderPreview(DrawingContext context)
            => ((IVisual)this).RenderSelfAndDescendants(context);

    }
}

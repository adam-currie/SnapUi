using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi {
    public class DraggableBorder : Border, IDraggable {
        private readonly DragImplementor impl;

        static DraggableBorder() {
            //make this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DraggableBorder>(Brushes.Transparent);
        }


        public DraggableBorder() {
            impl = new DragImplementor(this);
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

        public void RenderPreview(DrawingContext context, Point origin) {
            using (context.PushPreTransform(Matrix.CreateTranslation(origin))) {

                Render(context);
                
                foreach (var v in ((IVisual)this).GetVisualDescendants()) {
                    Matrix m = (Matrix)v.TransformToVisual(this)!;
                    using (context.PushPreTransform(m)) {
                        v.Render(context);
                    }
                }
            }
        }
    }
}

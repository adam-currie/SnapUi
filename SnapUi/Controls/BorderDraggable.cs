using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi.Controls {
    public class BorderDraggable : Border, IDraggable {
        private readonly IDraggable.DragImplementor impl;
        public event System.EventHandler? MeasureInvalidated;

        static BorderDraggable() {
            //make this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<BorderDraggable>(Brushes.Transparent);
        }

        public static readonly StyledProperty<int> MinDragDistanceProperty =
                AvaloniaProperty.Register<IDraggable, int>(nameof(MinDragDistance), 10, true);

        public int MinDragDistance {
            get { return GetValue(MinDragDistanceProperty); }
            set { SetValue(MinDragDistanceProperty, value); }
        }

        //todo: PreviewOpacity styled property

        public BorderDraggable() {
            impl = new IDraggable.DragImplementor(this, MakeMinDistDragOp);
        }

        private IDragOp MakeMinDistDragOp(IDraggable draggable, Point startingPoint)
            => new MinDistanceDragOp(draggable, startingPoint, MinDragDistance, MakePlainDragOp);

        private IDragOp MakePlainDragOp(IDraggable draggable, Point startingPoint)
            => new DragOp(draggable, startingPoint);

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

        void IDraggable.RenderPreview(DrawingContext context, IPreviewOfDraggable preview) {
            if (preview.IsFloating) {
                ((IVisual)this).RenderSelfAndDescendants(context);
            } else {
                ArrangeCore((Rect)preview.PreviousArrange!);
                ((IVisual)this).RenderSelfAndDescendants(context);
                ArrangeCore((Rect)((ILayoutable)this).PreviousArrange!);
            }
        }
        

        protected override void OnMeasureInvalidated() {
            base.OnMeasureInvalidated();
            MeasureInvalidated?.Invoke(this, System.EventArgs.Empty);
        }

        public Size PreviewMeasureOverride(Size availableSize)
            => MeasureOverride(availableSize);
    }
}

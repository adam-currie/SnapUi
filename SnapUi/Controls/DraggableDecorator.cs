using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi.Controls {
    public class DraggableDecorator : Decorator, IDraggable {
        private readonly IDraggable.DragImplementor dragImpl;

        public event System.EventHandler? MeasureInvalidated;

        public override void Render(DrawingContext context) {
            //make this hit-testable by default
            context.FillRectangle(Brushes.Transparent, new Rect(Bounds.Size));
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);
            //todo:  make sure this is in a dropzone
        }

        public static readonly StyledProperty<int> MinDragDistanceProperty =
                AvaloniaProperty.Register<IDraggable, int>(nameof(MinDragDistance), 10, true);

        public int MinDragDistance {
            get => GetValue(MinDragDistanceProperty);
            set => SetValue(MinDragDistanceProperty, value);
        }

        private IDragOp MakeMinDistDragOp(IDraggable draggable, Point startingPoint)
            => new MinDistanceDragOp(draggable, startingPoint, MinDragDistance, MakePlainDragOp);

        private IDragOp MakePlainDragOp(IDraggable draggable, Point startingPoint)
            => new DragOp(draggable, startingPoint);


        //todo: PreviewOpacity styled property

        public DraggableDecorator() {
            dragImpl = new IDraggable.DragImplementor(this, MakeMinDistDragOp);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e) {
            base.OnPointerPressed(e);
            dragImpl.DraggablePointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e) {
            base.OnPointerMoved(e);
            dragImpl.DraggablePointerMoved(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e) {
            base.OnPointerReleased(e);
            dragImpl.DraggablePointerReleased(e);
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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace SnapUi.Controls {
    internal class PreviewOfDraggable : Control, IPreviewOfDraggable {
        public IDraggable Draggable { get; }

        /// <summary>
        /// Indicates whether this is a floating preview or a drop preview.
        /// </summary>
        public bool IsFloating { get; }

        internal PreviewOfDraggable(IDraggable draggable, bool isFloating) {
            Draggable = draggable;
            IsFloating = isFloating;
            InvalidateVisual();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);
            Draggable.MeasureInvalidated += Draggable_MeasureInvalidated;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnDetachedFromVisualTree(e);
            Draggable.MeasureInvalidated -= Draggable_MeasureInvalidated;
        }

        private void Draggable_MeasureInvalidated(object? sender, System.EventArgs e) {
            InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize) {
            return Draggable.PreviewMeasureOverride(availableSize);
        }

        public override void Render(DrawingContext context) {
            base.Render(context);

            Draggable.RenderPreview(context, this);

            //todo: see if we can listen for invalidation of the target so we don't have to constantly invalidate
            InvalidateVisual();
        }

    }
}
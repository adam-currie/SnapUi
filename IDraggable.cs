using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace SnapUi {
    public interface IDraggable : IControl {

        public enum PreviewStyles {
            /// <summary>
            /// Shows the full control in the candidate drop zone.
            /// </summary>
            Full,
            /// <summary>
            /// Shows only a highlight of the controls area in the drop zone.
            /// </summary>
            Highlight
        }

        //todo: PreviewOpacity styled property

        public static readonly StyledProperty<PreviewStyles> PreviewStyleProperty =
            AvaloniaProperty.Register<IDraggable, PreviewStyles>(nameof(PreviewStyle), default, true);

        public PreviewStyles PreviewStyle {
            get { return GetValue(PreviewStyleProperty); }
            set { SetValue(PreviewStyleProperty, value); }
        }

        public void RenderPreview(DrawingContext context);

        public static readonly StyledProperty<int> MinDragDistanceProperty =
            AvaloniaProperty.Register<IDraggable, int>(nameof(MinDragDistance), 10, true);

        public int MinDragDistance {
            get { return GetValue(MinDragDistanceProperty); }
            set { SetValue(MinDragDistanceProperty, value); }
        }

    }
}
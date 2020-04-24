using Avalonia.Controls;

namespace SnapUi.Controls {
    public interface IPreviewOfDraggable : IControl {
        public IDraggable Draggable { get; }
        public bool IsFloating { get; }
    }
}

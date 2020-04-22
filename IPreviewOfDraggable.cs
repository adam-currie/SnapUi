using Avalonia.Controls;

namespace SnapUi {
    public interface IPreviewOfDraggable : IControl {
        public IDraggable Draggable { get; }
        public bool IsFloating { get; }
    }
}

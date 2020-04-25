using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace SnapUi.Controls {
    public interface IPreviewOfDraggable : IControl, ILogical {
        public IDraggable Draggable { get; }
        public bool IsFloating { get; }
    }
}

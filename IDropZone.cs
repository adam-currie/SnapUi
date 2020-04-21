using Avalonia.Controls;

namespace SnapUi {
    public interface IDropZone : IControl {
        public bool CanAdd(IDraggable draggable);
        public void Add(IDraggable draggable);
    }
}

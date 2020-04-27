using Avalonia.Controls;

namespace SnapUi.Controls {
    public interface IDropZone : IControl {
        public void AddPreview(IControl preview);
        public bool RemovePreview(IControl preview);
        public bool CanAddDraggable(IDraggable draggable);
        public void AddDraggable(IDraggable draggable);
        public bool RemoveDraggable(IDraggable draggable);
    }
}

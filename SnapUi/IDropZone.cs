using Avalonia.Controls;

namespace SnapUi {
    public interface IDropZone : IControl {
        public void AddPreview(IPreviewOfDraggable preview);
        public bool RemovePreview(IPreviewOfDraggable preview);
        public bool CanAddDraggable(IDraggable draggable);
        public void AddDraggable(IDraggable draggable);
        public bool RemoveDraggable(IDraggable draggable);
    }
}

using Avalonia.Controls;
using Avalonia.Media;
using System.Diagnostics;

namespace SnapUi.Controls {
    public class DropZoneStackPanel : StackPanel, IDropZone {

        //todo: check out Children.Validate

        static DropZoneStackPanel() {
            //making this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DropZoneStackPanel>(Brushes.Pink);//debug (Brushes.Transparent);
        }

        public void AddDraggable(IDraggable draggable) {
            Debug.Assert(CanAddDraggable(draggable));
            Children.Add(draggable);
        }

        public void AddPreview(IControl preview)
            => Children.Add(preview);

        public bool CanAddDraggable(IDraggable draggable)
            => true;

        public bool RemoveDraggable(IDraggable draggable)
            => Children.Remove(draggable);

        public bool RemovePreview(IControl preview)
            => Children.Remove(preview);
    }
}

using Avalonia.Controls;
using System.Diagnostics;
using Avalonia.Media;

namespace SnapUi {
    public class DropZoneStackPanel : StackPanel, IDropZone {

        static DropZoneStackPanel() {
            //make this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DropZoneStackPanel>(Brushes.Pink);//debug (Brushes.Transparent);
        }

        public void Add(IDraggable draggable) {
            Debug.Assert(CanAdd(draggable));

            Panel oldParent = ((Panel)draggable.Parent);//todo: support removing from any parent and maybe do this from drag op
            oldParent.Children.Remove(draggable);

            Children.Add(draggable);
        }

        public bool CanAdd(IDraggable draggable) => true;
    }
}

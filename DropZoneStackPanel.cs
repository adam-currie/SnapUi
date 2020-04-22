using Avalonia.Controls;
using System.Diagnostics;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi {
    public class DropZoneStackPanel : StackPanel, IDropZone {

        static DropZoneStackPanel() {
            //making this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DropZoneStackPanel>(Brushes.Pink);//debug (Brushes.Transparent);
        }

        public void Add(IDraggable draggable) {
            Debug.Assert(CanAdd(draggable));
            
            //todo: hide placeholder object

            Children.Add(draggable);
        }

        public bool CanAdd(IDraggable draggable) => true;

        public bool Remove(IDraggable draggable) {
            bool removed = Children.Remove(draggable);

            if (removed) {
                if (Children.Count == 0) {
                    //todo: put placeholder here instead so we can still drag things here
                    IsVisible = false;
                }
            }

            return removed;
        }
    }
}

using Avalonia.Controls;
using System.Diagnostics;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections.Specialized;

namespace SnapUi {
    public class DropZoneStackPanel : StackPanel, IDropZone {

        protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e) {
            base.ChildrenChanged(sender, e);
            IsVisible = Children.Count > 0;//todo: placeholder control
        }

        static DropZoneStackPanel() {
            //making this hit-testable by default
            BackgroundProperty.OverrideDefaultValue<DropZoneStackPanel>(Brushes.Pink);//debug (Brushes.Transparent);
        }

        public void AddDraggable(IDraggable draggable) {
            Debug.Assert(CanAddDraggable(draggable));
            Children.Add(draggable);
        }

        public void AddPreview(IPreviewOfDraggable preview)
            => Children.Add(preview);

        public bool CanAddDraggable(IDraggable draggable) 
            => true;

        public bool RemoveDraggable(IDraggable draggable) 
            => Children.Remove(draggable);

        public bool RemovePreview(IPreviewOfDraggable preview)
            => Children.Remove(preview);
    }
}

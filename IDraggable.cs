using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace SnapUi {
    public interface IDraggable : IControl {

        public enum PreviewStyles {
            /// <summary>
            /// Shows the full control in the candidate drop zone.
            /// </summary>
            Full,
            /// <summary>
            /// Shows only a highlight of the controls area in the drop zone.
            /// </summary>
            Highlight
        }

        //todo: PreviewOpacity styled property

        public static readonly StyledProperty<PreviewStyles> PreviewStyleProperty =
            AvaloniaProperty.Register<IDraggable, PreviewStyles>(nameof(PreviewStyle), default, true);

        public PreviewStyles PreviewStyle {
            get { return GetValue(PreviewStyleProperty); }
            set { SetValue(PreviewStyleProperty, value); }
        }

        public void RenderPreview(DrawingContext context);

        public static readonly StyledProperty<int> MinDragDistanceProperty =
            AvaloniaProperty.Register<IDraggable, int>(nameof(MinDragDistance), 10, true);

        public int MinDragDistance {
            get { return GetValue(MinDragDistanceProperty); }
            set { SetValue(MinDragDistanceProperty, value); }
        }

        public IDropZone DropZoneParent 
            => (Parent is IDropZone)? (IDropZone)Parent : throw new InvalidParentException(typeof(IDropZone));

        /// <summary>
        /// Implements common logic for starting drag operations.
        /// </summary>
        protected class DragImplementor {
            private readonly IDraggable draggable;
            private MinDistanceDragOp? preDragOp = null;

            public DragImplementor(IDraggable draggable) {
                this.draggable = draggable;
            }

            /* todo: maybe have to handle oncapturelost, doing some type of pointer release stuff, in case an ancestor steals control
             * https://github.com/AvaloniaUI/Avalonia/issues/2586
             */

            public void DraggablePointerPressed(PointerPressedEventArgs e) {
                /*
                 * if a descendant is pressed, 
                 * it will be the "captured" control for the life of the drag operation, 
                 * so rn we are transferring capture.
                 * if this ever doesn't work i think we can just fix it by listening for 
                 * InputElement.PointerMoved and InputElement.PointerReleased events even with the Handled property set to true
                 */

                if (preDragOp != null) {
                    /*  this is likely A: 
                     *      common(unavoidable?) issue where a pointer release event is not fired.
                     *  or B:
                     *      this is a different pointing device from the first.
                     *  EITHER WAY the easiest way to handle this is to just:
                     *      CANCEL the old drag.
                     */
                    preDragOp.Dispose();
                    preDragOp = null;
                    draggable.InvalidateVisual();
                    e.Pointer.Capture(null);
                } else {
                    if (e.Pointer.Captured != draggable) {
                        e.Pointer.Capture(draggable);
                    }
                    preDragOp = new MinDistanceDragOp(draggable, e.GetPosition(draggable));
                }

                e.Handled = true;
            }

            public void DraggablePointerMoved(PointerEventArgs e) {
                if (e.Pointer.Captured == draggable) {
                    preDragOp!.Update(e.GetPosition(draggable));
                    e.Handled = true;
                }
            }

            public void DraggablePointerReleased(PointerReleasedEventArgs e) {
                if (e.Pointer.Captured == draggable) {
                    preDragOp!.Release(e.GetPosition(draggable));
                    preDragOp = null;
                    e.Handled = true;
                }
            }

        }

    }
}
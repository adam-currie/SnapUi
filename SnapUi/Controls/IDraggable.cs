using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace SnapUi.Controls {
    public interface IDraggable : IControl {

        public event System.EventHandler MeasureInvalidated;

        protected internal void RenderPreview(DrawingContext context, IPreviewOfDraggable preview);

        public IDropZone DropZoneParent =>
            (LogicalParent is IDropZone parent) ?
                parent :
                throw new InvalidParentException(typeof(IDropZone));

        public Size PreviewMeasureOverride(Size availableSize);

        /// <summary>
        /// Implements common logic for starting drag operations.
        /// </summary>
        protected class DragImplementor {
            private readonly IDraggable draggable;
            private readonly IDragOp.Factory dragFactory;
            private IDragOp? dragOp = null;

            public DragImplementor(IDraggable draggable, IDragOp.Factory dragFactory) {
                this.draggable = draggable;
                this.dragFactory = dragFactory;

                draggable.DetachedFromVisualTree +=
                    (s, e) => Cancel();
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

                if (dragOp != null) {
                    /*  this is likely A: 
                     *      common(unavoidable?) issue where a pointer release event is not fired.
                     *  or B:
                     *      this is a different pointing device from the first.
                     *  EITHER WAY the easiest way to handle this is to just:
                     *      CANCEL the old drag.
                     */
                    dragOp.Dispose();
                    dragOp = null;
                    draggable.InvalidateVisual();
                    e.Pointer.Capture(null);
                } else {
                    if (e.Pointer.Captured != draggable) {
                        e.Pointer.Capture(draggable);
                    }
                    dragOp = dragFactory(draggable, e.GetPosition(draggable));
                }

                e.Handled = true;
            }

            public void DraggablePointerMoved(PointerEventArgs e) {
                if (e.Pointer.Captured == draggable) {
                    dragOp!.Update(e.GetPosition(draggable));
                    e.Handled = true;
                }
            }

            public void DraggablePointerReleased(PointerReleasedEventArgs e) {
                if (e.Pointer.Captured == draggable) {
                    dragOp!.Release(e.GetPosition(draggable));
                    dragOp = null;
                    e.Handled = true;
                }
            }

            /// <summary>
            /// Cancels current drag operation, if any.
            /// </summary>
            /// <remarks>
            /// This is called automatically when the draggable fires <see cref="IVisual.DetachedFromVisualTree"/>.
            /// </remarks>
            public void Cancel() => dragOp?.Dispose();

        }

    }
}
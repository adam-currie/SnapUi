﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using SnapUi.DragOps;
using System;
using System.Diagnostics;

namespace SnapUi.Controls {
    public class Draggable : Decorator, IDraggable {
        private readonly IDraggable.DragImplementor dragImpl;

        public event System.EventHandler? MeasureInvalidated;

        /// <summary>
        /// Declares what preview states a control and it's descendants are visible for.
        /// </summary>
        /// <remarks>
        /// IsVisible is ignored for preview rendering.
        /// </remarks>
        public enum PreviewVisibility {
            All = 0,
            None,
            FloatingPreview,
            DropPreview
        }

        public static readonly AttachedProperty<PreviewVisibility> PreviewVisibilityProperty =
            AvaloniaProperty.RegisterAttached<Draggable, Control, PreviewVisibility>(name: "PreviewVisibility", inherits: true);

        public override void Render(DrawingContext context) {
            //make this hit-testable by default
            context.FillRectangle(Brushes.Transparent, new Rect(Bounds.Size));
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e) {
            base.OnAttachedToVisualTree(e);
            //todo:  make sure this is in a dropzone
        }

        public static readonly StyledProperty<int> MinDragDistanceProperty =
                AvaloniaProperty.Register<IDraggable, int>(nameof(MinDragDistance), 10, true);

        public int MinDragDistance {
            get => GetValue(MinDragDistanceProperty);
            set => SetValue(MinDragDistanceProperty, value);
        }

        private IDragOp MakeDragOp(IDraggable draggable, Point startingPoint)
            => new MinDistanceDragOp(draggable, startingPoint, MinDragDistance, 
                (d,s) => new DragOp(d,s));


        //todo: PreviewOpacity styled property

        public Draggable() {
            dragImpl = new IDraggable.DragImplementor(this, MakeDragOp);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e) {
            base.OnPointerPressed(e);
            dragImpl.DraggablePointerPressed(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e) {
            base.OnPointerMoved(e);
            dragImpl.DraggablePointerMoved(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e) {
            base.OnPointerReleased(e);
            dragImpl.DraggablePointerReleased(e);
        }

        protected override void OnMeasureInvalidated() {
            base.OnMeasureInvalidated();
            MeasureInvalidated?.Invoke(this, System.EventArgs.Empty);
        }

        public Size RemoteRenderMeasureOverride(Size remoteAvailableSize) {
            Size? realAvailableSize = ((ILayoutable)this).PreviousMeasure;//todo: handle this being null?

            return (remoteAvailableSize == realAvailableSize) ?
                DesiredSize :
                MeasureOverride(remoteAvailableSize);
        }

        public void RemoteRender(RemoteViewer host, DrawingContext context, Rect temporaryArrangeCoreRect) {
            ArrangeCore(temporaryArrangeCoreRect!);

            //todo: if host is DragPreviewer, temporarily make visible all the descendants with the right PreviewVisibility property
            ((IVisual)this).RenderSelfAndDescendants(context);

            ArrangeCore((Rect)((ILayoutable)this).PreviousArrange!);
        }

    }
}

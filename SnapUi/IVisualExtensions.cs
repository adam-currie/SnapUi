using Avalonia;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Linq;

namespace SnapUi {
    internal static class IVisualExtensions {

        /// <summary>
        /// Gets the first visual ancestor of this type.
        /// </summary>
        /// <typeparam name="T">the type of the required ancestor</typeparam>
        /// <exception cref="RequiredAncestorNotFoundException"></exception>
        public static T GetVisualAncestor<T>(this IVisual v) {
            try {
                return (T)v.GetVisualAncestors()
                    .First((v) => v is T);
            } catch (InvalidOperationException) {
                throw new RequiredAncestorNotFoundException(typeof(T));
            }
        }

        public static void RenderSelfAndDescendants(this IVisual v, DrawingContext context) {
            foreach (var c in v.GetSelfAndVisualDescendants()) {
                Matrix m = (Matrix)c.TransformToVisual(v)!;
                using (context.PushPreTransform(m)) {
                    c.Render(context);
                }
            }
        }

    }
}

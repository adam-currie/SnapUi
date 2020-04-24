using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace SnapUi.Controls {

    /// <summary>
    /// Root element to implicitly limit the scope of drag/drop operations.
    /// </summary>
    public interface ISnapUiRoot : IControl {
         public OverlayLayer OverlayLayer { get; }
    }
}

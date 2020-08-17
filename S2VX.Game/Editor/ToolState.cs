using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor {
    // For more flexibility in handling a selected editor tool, we can use a class
    // instead of a simpler enum. See https://gameprogrammingpatterns.com/state.html
    // for some background and additional details.
    public abstract class ToolState : Container {
        public abstract string DisplayName();
        // Returns whether to block propagation. By default, return false
        // to allow a parent component, i.e. S2VXEditor, to handle inputs.
        public virtual bool OnToolClick(ClickEvent e) => false;
    }
}

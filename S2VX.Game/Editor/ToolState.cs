using System;
using System.Collections.Generic;
using System.Text;

namespace S2VX.Game.Editor
{
    // For more flexibility in handling a selected editor tool, we can use a class
    // instead of a simpler enum. See https://gameprogrammingpatterns.com/state.html
    // for some background and additional details.
    public abstract class ToolState
    {
        // These handlers return whether to block propagation. By default, we return
        // false to allow a parent component, e.g. S2VXEditor, to handle inputs.
        public virtual bool OnMouseMove()
        {
            return false;
        }

        public virtual bool OnMouseClick()
        {
            return false;
        }

        public virtual void Update() {
        }
    }
}

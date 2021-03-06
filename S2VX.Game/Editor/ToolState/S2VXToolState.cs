﻿using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.ToolState {
    // For more flexibility in handling a selected editor tool, we can use a class
    // instead of a simpler enum. See https://gameprogrammingpatterns.com/state.html
    // for some background and additional details.
    public abstract class S2VXToolState : Container {
        public abstract string DisplayName();
        // Returns whether to block propagation. By default, return false
        // to allow a parent component, i.e. S2VXEditor, to handle inputs.
        public virtual bool OnToolClick(ClickEvent e) => false;
        public virtual bool OnToolMouseDown(MouseDownEvent e) => false;
        public virtual void OnToolMouseMove(MouseMoveEvent e) { }
        public virtual void OnToolMouseUp(MouseUpEvent e) { }
        public virtual bool OnToolDragStart(DragStartEvent e) => false;
        public virtual void OnToolDrag(DragEvent e) { }
        public virtual void OnToolDragEnd(DragEndEvent e) { }
        public virtual bool OnToolKeyDown(KeyDownEvent e) => false;
        public virtual void HandleExit() { }
    }
}

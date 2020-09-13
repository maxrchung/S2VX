using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.ToolState {
    public class CameraToolState : S2VXToolState {
        CameraToolDragState DragState = CameraToolDragState.None;
        Vector2 OldPosition { get; set; }
        double OldTime { get; set; }
        Vector2 DragStart { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        private void RememberOldValues() {
            OldPosition = Editor.MousePosition;
            OldTime = Editor.Track.CurrentTime;
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.ControlLeft:
                    DragState = CameraToolDragState.Move;
                    RememberOldValues();
                    return true;
                case Key.ShiftLeft:
                    DragState = CameraToolDragState.Scale;
                    RememberOldValues();
                    return true;
                case Key.AltLeft:
                    DragState = CameraToolDragState.Rotate;
                    RememberOldValues();
                    return true;
            }
            return false;
        }

        public override bool OnToolDragStart(DragStartEvent e) {
            DragStart = e.MousePosition;
        }

        public override void OnToolDrag(DragEvent e) {

        }

        public override void OnToolDragEnd(DragEndEvent e) {
            var time = Editor.Track.CurrentTime;
            // Don't add command if at the same time
            if (time == OldTime) {
                return;
            }

            var startTime = OldTime;
            var endTime = time;
            // Allows commands when going backwards or forwards
            if (time < OldTime) {
                startTime = time;
                endTime = OldTime;
            }

            switch (DragState) {
                case CameraToolDragState.Move:
                    return;
                case CameraToolDragState.Scale:
                    return;
                case CameraToolDragState.Rotate:
                    return;
            }
        }

        public override string DisplayName() => "Camera";
    }
}

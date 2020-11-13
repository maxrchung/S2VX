using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.ToolState {
    public class HoldNoteToolState : S2VXToolState {
        private EditorHoldNote Preview { get; set; } = new EditorHoldNote();

        [Resolved]
        private EditorScreen Editor { get; set; } = null;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private bool IsRecording { get; set; }
        private Vector2 Coordinates { get; set; }
        private double HitTime { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;
            Child = Preview;
        }

        public override bool OnToolClick(ClickEvent e) {
            if (!IsRecording) {
                HitTime = Time.Current;
                Coordinates = Editor.MousePosition;
            } else {
                var endTime = Time.Current;
                if (endTime > HitTime) {
                    var note = new EditorHoldNote {
                        Coordinates = Coordinates,
                        HitTime = HitTime,
                        EndTime = endTime
                    };
                    Editor.Reversibles.Push(new ReversibleAddHoldNote(Story, note));
                }
            }

            IsRecording = !IsRecording;
            return true;
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    if (IsRecording) {
                        IsRecording = false;
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        protected override void Update() {
            Preview.Colour = Story.Notes.Colour;

            if (!IsRecording) {
                Preview.Coordinates = Editor.MousePosition;
                Preview.HitTime = Time.Current;
                Preview.EndTime = Time.Current;
            } else {
                Preview.Coordinates = Coordinates;
                Preview.HitTime = HitTime;
                Preview.EndTime = Time.Current;
            }
        }

        public override string DisplayName() =>
            IsRecording ? "Hold Note (Click to End, Esc to Cancel)" : "Hold Note (Click to Start)";
    }
}

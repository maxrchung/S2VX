using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
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

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;
            Child = Preview;
        }

        public override bool OnToolClick(ClickEvent _) {
            var note = new EditorHoldNote {
                Coordinates = Editor.MousePosition,
                HitTime = Time.Current,
                EndTime = Time.Current + 1000    // TODO: #216 be able to change hold duration
            };
            Editor.Reversibles.Push(new ReversibleAddHoldNote(Story, note));
            return false;
        }

        protected override void Update() {
            Preview.EndTime = Time.Current;
            Preview.Coordinates = Editor.MousePosition;
            Preview.Colour = Story.Notes.Colour;
        }

        public override string DisplayName() => "Hold Note";
    }
}

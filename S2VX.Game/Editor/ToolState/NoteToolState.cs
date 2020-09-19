using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.ToolState {
    public class NoteToolState : S2VXToolState {
        private S2VXNote Preview { get; set; } = new S2VXNote();

        [Resolved]
        private S2VXEditor Editor { get; set; } = null;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;
            Child = Preview;
        }

        public override bool OnToolClick(ClickEvent _) {
            var note = new S2VXNote {
                Coordinates = Editor.MousePosition,
                EndTime = Time.Current
            };
            Editor.Reversibles.Push(new ReversibleAddNote(Story, note));
            return false;
        }

        protected override void Update() {
            Preview.EndTime = Time.Current;
            Preview.Coordinates = Editor.MousePosition;
            Preview.Colour = Story.Notes.Colour;
        }

        public override string DisplayName() => "Note";
    }
}

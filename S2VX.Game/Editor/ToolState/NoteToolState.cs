using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.ToolState {
    public class NoteToolState : S2VXToolState {
        private Note Preview { get; set; } = new Note();

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
            var note = new Note {
                Coordinates = Editor.MousePosition,
                EndTime = Time.Current
            };
            Editor.Reversibles.Push(new ReversibleAddNote(Story, note));
            return false;
        }

        protected override void Update() {
            Preview.EndTime = Time.Current;
            Preview.Coordinates = Editor.MousePosition;
        }

        public override string DisplayName() => "Note";
    }
}

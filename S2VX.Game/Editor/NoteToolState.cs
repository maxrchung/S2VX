using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;
using System.Text;

namespace S2VX.Game.Editor {
    public class NoteToolState : ToolState {
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
            Story.AddNote(Editor.MousePosition, Story.GameTime);
            return false;
        }

        protected override void Update() {
            Preview.EndTime = Story.GameTime;
            Preview.Coordinates = Editor.MousePosition;
        }
    }
}

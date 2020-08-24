﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Story;

namespace S2VX.Game.Editor {
    public class NoteToolState : ToolState {
        private Note Preview { get; set; } = new Note();

        [Resolved]
        private S2VXEditor Editor { get; set; } = null;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
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

        public override string DisplayName() => "Note";
    }
}

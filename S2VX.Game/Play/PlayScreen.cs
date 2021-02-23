﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Play.Containers;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;
using S2VX.Game.Story.Play.Containers;

namespace S2VX.Game.Play {
    public class PlayScreen : Screen {
        // Flag denoting whether (true) to use a story's editor settings or
        // (false) to start at 0
        private bool IsUsingEditorSettings { get; }

        private S2VXStory Story { get; }
        private DrawableTrack Track { get; }

        public PlayInfoBar PlayInfoBar { get; private set; } = new PlayInfoBar();

        public PlayScreen(bool isUsingEditorSettings, S2VXStory story, DrawableTrack track) {
            IsUsingEditorSettings = isUsingEditorSettings;
            Story = story;
            Track = track;
        }

        public ScoreInfo ScoreInfo { get; private set; }

        // Need to explicitly recache screen since new ones can be recreated
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var dependencies = new DependencyContainer(parent);
            dependencies.Cache(this);
            dependencies.Cache(Story);
            // ScoreInfo needs to be initialized here so that it is cached before GameNotes need it
            dependencies.Cache(ScoreInfo = new ScoreInfo {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            });
            dependencies.Cache(PlayInfoBar = new PlayInfoBar());
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Story.ClearActives();

            if (IsUsingEditorSettings) {
                var settings = Story.EditorSettings;
                var trackTime = settings.TrackTime;
                Track.Seek(trackTime);
                Story.RemoveNotesUpTo(trackTime);
            }

            Clock = new FramedClock(Track);
            InternalChildren = new Drawable[] {
                new InputKeyBindingContainer {
                    Story
                },
                Track,
                PlayInfoBar,
            };

            Track.Start();
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Tab:
                    if (e.ShiftPressed) {
                        PlayInfoBar.Alpha = 1 - PlayInfoBar.Alpha;
                    }
                    break;
                case Key.Escape:
                    this.Push(new LeaveScreen());
                    return true;
            }
            return false;
        }
    }
}

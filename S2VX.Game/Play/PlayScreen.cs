using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Play.Containers;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;

namespace S2VX.Game.Play {
    public class PlayScreen : Screen {
        private S2VXStory Story { get; set; }

        public PlayInfoBar PlayInfoBar { get; private set; } = new PlayInfoBar();

        // Need to explicitly recache screens since new ones can be recreated
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var dependencies = new DependencyContainer(parent);
            dependencies.Cache(this);
            dependencies.Cache(Story = new S2VXStory());
            // ScoreInfo needs to be initialized here so that it is cached before GameNotes need it
            dependencies.Cache(new ScoreInfo {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            });
            dependencies.Cache(PlayInfoBar = new PlayInfoBar());
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Story.Open(@"../../../story.json", false);
            Story.ClearActives();

            var track = new DrawableTrack(audio.Tracks.Get(@"Camellia_MEGALOVANIA_Remix.mp3"));
            var settings = Story.GetEditorSettings();
            var trackTime = settings.TrackTime;
            track.Tempo.Value = settings.TrackPlaybackRate;
            track.Seek(trackTime);
            Story.RemoveNotesUpTo(trackTime);

            track.Start();
            Clock = new FramedClock(track);
            InternalChildren = new Drawable[] {
                Story,
                track,
                PlayInfoBar,
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Tab:
                    if (e.ShiftPressed) {
                        PlayInfoBar.Alpha = 1 - PlayInfoBar.Alpha;
                    }
                    break;
                case Key.Escape:
                    this.Exit();
                    return true;
            }
            return false;
        }
    }
}

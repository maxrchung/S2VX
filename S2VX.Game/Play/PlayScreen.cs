using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Play.Containers;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;

namespace S2VX.Game.Play {
    public class PlayScreen : Screen {
        private string AudioDir { get; set; }
        private StorageBackedResourceStore CurLevelResourceStore { get; set; }
        private string CurSelectionPath { get; set; }
        private string StoryDir { get; set; }
        private string FullStoryDir { get; set; }
        public PlayScreen(bool isUsingEditorSettings, string curSelectionPath, string storyDir,
            StorageBackedResourceStore curLevelResourceStore, string audioDir) {

            IsUsingEditorSettings = isUsingEditorSettings;
            CurSelectionPath = curSelectionPath;
            StoryDir = storyDir;
            AudioDir = audioDir;
            CurLevelResourceStore = curLevelResourceStore;
            FullStoryDir = CurSelectionPath + "/" + StoryDir;
        }

        // Flag denoting whether (true) to use a story's editor settings or
        // (false) to start at 0
        private bool IsUsingEditorSettings { get; set; }

        public PlayScreen(bool isUsingEditorSettings) => IsUsingEditorSettings = isUsingEditorSettings;

        private S2VXStory Story { get; set; }

        public PlayInfoBar PlayInfoBar { get; private set; } = new PlayInfoBar();

        // Need to explicitly recache screen since new ones can be recreated
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
            Story.Open(FullStoryDir, false);
            Story.ClearActives();

            var trackStream = CurLevelResourceStore.GetStream(AudioDir);
            var trackBass = new TrackBass(trackStream);
            audio.AddItem(trackBass);
            var track = new DrawableTrack(trackBass);
            if (IsUsingEditorSettings) {
                var settings = Story.GetEditorSettings();
                var trackTime = settings.TrackTime;
                track.Seek(trackTime);
                Story.Notes.RemoveNotesUpTo(trackTime);
            }

            Clock = new FramedClock(track);
            InternalChildren = new Drawable[] {
                Story,
                track,
                PlayInfoBar,
            };

            track.Start();
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

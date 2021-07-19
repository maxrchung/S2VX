using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Configuration;
using S2VX.Game.EndGame;
using S2VX.Game.Play.Containers;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Play {
    public class PlayScreen : Screen {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        // Flag denoting whether (true) to use a story's editor settings or
        // (false) to start at 0
        private bool IsUsingEditorSettings { get; }

        private S2VXStory Story { get; }
        private DrawableTrack Track { get; }

        public HitErrorBar HitErrorBar { get; private set; }
        public Bindable<bool> ConfigHitErrorBarVisibility { get; set; } = new();
        public Bindable<bool> ConfigScoreVisibility { get; set; } = new();

        [Resolved]
        private GlobalVolumeDisplay VolumeDisplay { get; set; }

        public PlayScreen(bool isUsingEditorSettings, S2VXStory story, DrawableTrack track) {
            IsUsingEditorSettings = isUsingEditorSettings;
            Story = story;
            Track = track;
            track.Completed += OnTrackCompleted;
        }

        public ScoreProcessor ScoreProcessor { get; private set; }
        private const float InfoBarHeight = 0.06f;
        private const float InfoBarWidth = 1.0f;

        // Need to explicitly recache screen since new ones can be recreated
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var dependencies = new DependencyContainer(parent);
            dependencies.Cache(this);
            dependencies.Cache(Story);
            // ScoreInfo needs to be initialized here so that it is cached before GameNotes need it
            dependencies.Cache(ScoreProcessor = new ScoreProcessor {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Height = InfoBarHeight,
                Width = InfoBarWidth,
            });
            dependencies.Cache(HitErrorBar = new HitErrorBar { Alpha = 0, });
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void Load(S2VXConfigManager config) {
            Story.ClearActives();

            if (IsUsingEditorSettings) {
                var settings = Story.EditorSettings;
                var trackTime = settings.TrackTime;
                Track.Seek(trackTime);
                Story.RemoveNotesUpTo(trackTime);
            }

            Clock = new FramedClock(Track);
            InternalChildren = new Drawable[] {
                new PlayKeyBindingContainer(this) {
                    Story
                },
                Track,
                HitErrorBar,
                ScoreProcessor
            };

            Track.Start();

            ConfigHitErrorBarVisibility = config.GetBindable<bool>(S2VXSetting.HitErrorBarVisibility);
            ConfigScoreVisibility = config.GetBindable<bool>(S2VXSetting.ScoreVisibility);

        }

        // Apply visibility of HitErrorBar based on the saved value
        protected override void LoadComplete() {
            ConfigHitErrorBarVisibility.BindValueChanged(_ => UpdateErrorBarVisibility(), true);
            ConfigScoreVisibility.BindValueChanged(_ => UpdateScoreVisibility(), true);
        }

        // Action of the bindable: this is where we actually set the HitErrorBar's alpha
        private void UpdateErrorBarVisibility() => HitErrorBar.Alpha = ConfigHitErrorBarVisibility.Value ? 1 : 0;

        // Action of the bindable: this is where we actually set the Score's alpha
        private void UpdateScoreVisibility() => ScoreProcessor.Alpha = ConfigScoreVisibility.Value ? 1 : 0;

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    this.Push(new LeaveScreen());
                    return true;
            }
            return false;
        }

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                VolumeDisplay.VolumeIncrease();
            } else {
                VolumeDisplay.VolumeDecrease();
            }
            VolumeDisplay.UpdateDisplay();
            return false;
        }

        public override bool OnExiting(IScreen next) {
            Cursor.Reset();
            return false;
        }

        public void OnTrackCompleted() {
            if (!IsUsingEditorSettings) {
                this.MakeCurrent();
                var storyDirectory = Path.GetDirectoryName(Story.StoryPath);
                this.Push(new EndGameScreen(ScoreProcessor.ScoreStatistics, storyDirectory));
            }
        }
    }
}

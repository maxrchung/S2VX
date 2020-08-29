using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Story;

namespace S2VX.Game.Editor {
    public class S2VXEditor : CompositeDrawable {
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();

        private CommandPanel CommandPanel { get; } = new CommandPanel();
        private bool IsCommandPanelVisible { get; set; }

        public NotesTimeline NotesTimeline { get; } = new NotesTimeline();
        private Timeline Timeline { get; } = new Timeline();

        public ToolState ToolState { get; private set; } = new SelectToolState();

        public Container NoteSelectionIndicators { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };

        private void SetToolState(ToolState newState) {
            ToolState.HandleExit();
            ToolState = newState;
        }

        private Container ToolContainer { get; set; } = new Container {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };

        [Resolved]
        private AudioManager Audio { get; set; }
        public DrawableTrack Track { get; private set; }

        private VolumeDisplay VolumeDisplay { get; set; } = new VolumeDisplay();

        [BackgroundDependencyLoader]
        private void Load() {
            Track = new DrawableTrack(Audio.Tracks.Get(@"Camellia_MEGALOVANIA_Remix.mp3"));
            Track.VolumeTo(0.10f);

            Story.Open(@"../../../story.json");

            var trackBasedClock = new FramedClock(Track);
            Clock = trackBasedClock;

            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            ToolContainer.Child = ToolState;
            InternalChildren = new Drawable[]
            {
                Story,
                NoteSelectionIndicators,
                ToolContainer,
                new ToolDisplay(),
                VolumeDisplay,
                NotesTimeline,
                new BasicMenu(Direction.Horizontal, true)
                {
                    BackgroundColour = Color4.Black.Opacity(0.9f),
                    Width = 1,
                    RelativeSizeAxes = Axes.X,
                    Items = new[]
                    {
                        new MenuItem("Project")
                        {
                            Items = new[]
                            {
                                new MenuItem("Refresh (Ctrl+R)", ProjectRefresh),
                                new MenuItem("Save (Ctrl+S)", ProjectSave)
                            }
                        },
                        new MenuItem("View")
                        {
                            Items = new[]
                            {
                                new MenuItem("Command Panel (Ctrl+1)", ViewCommandPanel),
                            }
                        },
                        new MenuItem("Playback")
                        {
                            Items = new[]
                            {
                                new MenuItem("Play/Pause (Space)", PlaybackPlay),
                                new MenuItem("Restart (X)", PlaybackRestart),
                                new MenuItem("Toggle Time Display (T)", PlaybackDisplay),
                                new MenuItem("Seek Left Tick (<- / MouseWheelUp)", PlaybackSeekLeftTick),
                                new MenuItem("Seek Right Tick (-> / MouseWheelDown)", PlaybackSeekRightTick),
                                new MenuItem("Zoom Out Notes Timeline (Ctrl+[)", PlaybackZoomOut),
                                new MenuItem("Zoom In Notes Timeline (Ctrl+])", PlaybackZoomIn),
                                new MenuItem("Decrease Beat Snap Divisor (Ctrl+Shift+[)", PlaybackDecreaseBeatDivisor),
                                new MenuItem("Increase Beat Snap Divisor (Ctrl+Shift+])", PlaybackIncreaseBeatDivisor),
                                new MenuItem("Decrease Volume (MouseWheelDown over Volume)", VolumeDecrease),
                                new MenuItem("Increase Volume (MouseWheelUp over Volume)", VolumeIncrease),
                            }
                        },
                        new MenuItem("Tool")
                        {
                            Items = new[]
                            {
                                new MenuItem("Select (1)", ToolSelect),
                                new MenuItem("Note (2)", ToolNote),
                            }
                        }
                    }
                },
                Timeline,
                CommandPanel
            };
        }

        protected override bool OnClick(ClickEvent e) => ToolState.OnToolClick(e);

        protected override bool OnMouseDown(MouseDownEvent e) => ToolState.OnToolMouseDown(e);

        protected override bool OnDragStart(DragStartEvent e) => ToolState.OnToolDragStart(e);

        protected override void OnDrag(DragEvent e) => ToolState.OnToolDrag(e);

        protected override void OnDragEnd(DragEndEvent e) => ToolState.OnToolDragEnd(e);

        protected override bool OnMouseMove(MouseMoveEvent e) {
            var mousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            var relativePosition = (mousePosition - Story.DrawSize / 2) / Story.DrawWidth;
            var camera = Story.Camera;
            var rotatedPosition = S2VXUtils.Rotate(relativePosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var translatedPosition = scaledPosition + camera.Position;
            MousePosition = translatedPosition;
            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (ToolState.OnToolKeyDown(e)) {
                return true;
            }
            switch (e.Key) {
                case Key.R:
                    if (e.ControlPressed) {
                        ProjectRefresh();
                    }
                    break;
                case Key.S:
                    if (e.ControlPressed) {
                        ProjectSave();
                    }
                    break;
                case Key.Number1: {
                    if (e.ControlPressed) {
                        ViewCommandPanel();
                    } else {
                        ToolSelect();
                    }
                    break;
                }
                case Key.Number2: {
                    ToolNote();
                    break;
                }
                case Key.BracketLeft: {
                    if (e.ControlPressed) {
                        if (e.ShiftPressed) {
                            PlaybackDecreaseBeatDivisor();
                        } else {
                            PlaybackZoomIn();
                        }
                    }
                    break;
                }
                case Key.BracketRight: {
                    if (e.ControlPressed) {
                        if (e.ShiftPressed) {
                            PlaybackIncreaseBeatDivisor();
                        } else {
                            PlaybackZoomOut();
                        }
                    }
                    break;
                }
                case Key.Space:
                    PlaybackPlay();
                    break;
                case Key.X:
                    PlaybackRestart();
                    break;
                case Key.T:
                    PlaybackDisplay();
                    break;
                case Key.Left:
                    NotesTimeline.SnapToTick(true);
                    break;
                case Key.Right:
                    NotesTimeline.SnapToTick(false);
                    break;
                default:
                    break;
            }
            return true;
        }

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                NotesTimeline.SnapToTick(true);
            } else {
                NotesTimeline.SnapToTick(false);
            }
            return true;
        }

        public void Play(bool isPlaying) {
            if (isPlaying) {
                Track.Start();
            } else {
                Track.Stop();
            }
        }

        public void Restart() {
            Track.Restart();
            Story.ClearActives();
        }

        public void Seek(double time) {
            Track.Seek(time);
            Story.ClearActives();
        }

        private void ProjectRefresh() {
            Play(false);
            Story.Save(@"../../../story.json");
            Story.Open(@"../../../story.json");
            Seek(Track.CurrentTime);
        }

        private void ProjectSave() {
            Play(false);
            Story.Save(@"../../../story.json");
        }

        private void ViewCommandPanel() {
            if (IsCommandPanelVisible) {
                CommandPanel.Hide();
            } else {
                CommandPanel.Show();
            }
            IsCommandPanelVisible = !IsCommandPanelVisible;
        }

        private void PlaybackPlay() {
            if (Time.Current < Track.Length) {
                Play(!Track.IsRunning);
            }
        }

        private void PlaybackRestart() => Restart();

        private void PlaybackDisplay() => Timeline.DisplayMS = !Timeline.DisplayMS;

        private void PlaybackSeekLeftTick() => NotesTimeline.SnapToTick(true);

        private void PlaybackSeekRightTick() => NotesTimeline.SnapToTick(false);

        private void PlaybackZoomOut() => NotesTimeline.HandleZoom(false);

        private void PlaybackZoomIn() => NotesTimeline.HandleZoom(true);

        private void PlaybackDecreaseBeatDivisor() => NotesTimeline.ChangeBeatDivisor(false);

        private void PlaybackIncreaseBeatDivisor() => NotesTimeline.ChangeBeatDivisor(true);

        private void VolumeIncrease() => VolumeIncrease(0.25);

        private void VolumeDecrease() => VolumeDecrease(0.25);

        public void VolumeIncrease(double step = 0.1) => Track.VolumeTo(Track.Volume.Value + step);

        public void VolumeDecrease(double step = 0.1) => Track.VolumeTo(Track.Volume.Value - step);

        private void ToolSelect() {
            SetToolState(new SelectToolState());
            ToolContainer.Child = ToolState;
        }

        private void ToolNote() {
            SetToolState(new NoteToolState());
            ToolContainer.Child = ToolState;
        }
    }
}

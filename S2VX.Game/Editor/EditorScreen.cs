using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Editor.ToolState;
using S2VX.Game.Play;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor {
    public class EditorScreen : Screen {
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();

        public CommandPanel CommandPanel { get; } = new CommandPanel();
        private bool IsCommandPanelVisible { get; set; }

        public NotesTimeline NotesTimeline { get; } = new NotesTimeline();

        private Timeline Timeline { get; } = new Timeline();

        public S2VXToolState ToolState { get; private set; } = new SelectToolState();

        public Container NoteSelectionIndicators { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };

        private void SetToolState(S2VXToolState newState) {
            ToolState.HandleExit();
            ToolState = newState;
            ToolContainer.Child = ToolState;
        }

        private Container ToolContainer { get; set; } = new Container {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };

        [Resolved]
        private AudioManager Audio { get; set; }
        public DrawableTrack Track { get; private set; }

        public ReversibleStack Reversibles { get; } = new ReversibleStack();

        public int SnapDivisor { get; private set; }
        private const int MaxSnapDivisor = 16;

        [Resolved]
        private ScreenStack Screens { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Story.Open(@"../../../story.json", true);

            Track = new DrawableTrack(Audio.Tracks.Get("Camellia_MEGALOVANIA_Remix"));
            Seek(Story.GetEditorSettings().TrackTime);
            PlaybackSetRate(Story.GetEditorSettings().TrackPlaybackRate);
            SnapDivisor = Story.GetEditorSettings().SnapDivisor;
            NotesTimeline.DivisorIndex = Story.GetEditorSettings().BeatSnapDivisorIndex;

            // Sets the same clock for sections dependent on the Track
            var trackClock = new FramedClock(Track);
            Story.Clock = trackClock;
            NotesTimeline.Clock = trackClock;
            Timeline.Clock = trackClock;
            ToolContainer.Clock = trackClock;

            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            ToolContainer.Child = ToolState;
            InternalChildren = new Drawable[]
            {
                Story,
                NoteSelectionIndicators,
                ToolContainer,
                NotesTimeline,
                new EditorInfoBar(),
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
                                new MenuItem("Save (Ctrl+S)", ProjectSave),
                                new MenuItem("Save (Ctrl+P)", ProjectPlay)
                            }
                        },
                        new MenuItem("Edit")
                        {
                            Items = new[]
                            {
                                new MenuItem("Undo (Ctrl+Z)", EditUndo),
                                new MenuItem("Redo (Ctrl+Shift+Z)", EditRedo)
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
                                new MenuItem("Seek Left Tick (Left / MouseWheelUp)", PlaybackSeekLeftTick),
                                new MenuItem("Seek Right Tick (Right / MouseWheelDown)", PlaybackSeekRightTick),
                                new MenuItem("Zoom Out Notes Timeline (Ctrl+[)", PlaybackZoomOut),
                                new MenuItem("Zoom In Notes Timeline (Ctrl+])", PlaybackZoomIn),
                                new MenuItem("Decrease Beat Snap Divisor (Ctrl+Shift+[)", PlaybackDecreaseBeatDivisor),
                                new MenuItem("Increase Beat Snap Divisor (Ctrl+Shift+])", PlaybackIncreaseBeatDivisor),
                                new MenuItem("Decrease Playback Speed (Down, MouseWheelDown over Speed)", PlaybackDecreaseRate),
                                new MenuItem("Increase Playback Speed (Up,  MouseWheelUp over Speed)", PlaybackIncreaseRate),
                                new MenuItem("Decrease Volume (MouseWheelDown over Volume)", VolumeDecrease),
                                new MenuItem("Increase Volume (MouseWheelUp over Volume)", VolumeIncrease),
                                new MenuItem("Decrease Snapping Divisor (MouseWheelDown over Snap Divisor)", SnapDivisorDecrease),
                                new MenuItem("Increase Snapping Divisor (MouseWheelUp over Snap Divisor)", SnapDivisorIncrease),
                            }
                        },
                        new MenuItem("Tool")
                        {
                            Items = new[]
                            {
                                new MenuItem("Select (1)", ToolSelect),
                                new MenuItem("Note (2)", ToolNote),
                                new MenuItem("Camera (3)", ToolCamera),
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

        protected override void OnMouseUp(MouseUpEvent e) => ToolState.OnToolMouseUp(e);

        protected override bool OnDragStart(DragStartEvent e) => ToolState.OnToolDragStart(e);

        protected override void OnDrag(DragEvent e) => ToolState.OnToolDrag(e);

        protected override void OnDragEnd(DragEndEvent e) => ToolState.OnToolDragEnd(e);

        protected override bool OnMouseMove(MouseMoveEvent e) {
            ToolState.OnToolMouseMove(e);

            var mousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            var relativePosition = (mousePosition - Story.DrawSize / 2) / Story.DrawWidth;
            var camera = Story.Camera;
            var rotatedPosition = S2VXUtils.Rotate(relativePosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var translatedPosition = scaledPosition + camera.Position;
            if (SnapDivisor == 0) {
                MousePosition = translatedPosition;
            } else {
                var closestSnap = new Vector2(
                    (float)(Math.Round(translatedPosition.X * SnapDivisor) / SnapDivisor),
                    (float)(Math.Round(translatedPosition.Y * SnapDivisor) / SnapDivisor)
                );
                MousePosition = closestSnap;
            }
            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (ToolState.OnToolKeyDown(e)) {
                return true;
            }
            switch (e.Key) {
                case Key.P:
                    if (e.ControlPressed) {
                        ProjectPlay();
                    }
                    break;
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
                case Key.Z:
                    if (e.ControlPressed) {
                        if (e.ShiftPressed) {
                            EditRedo();
                            break;
                        }
                        EditUndo();
                    }
                    break;
                case Key.Number1:
                    if (e.ControlPressed) {
                        ViewCommandPanel();
                    } else {
                        ToolSelect();
                    }
                    break;
                case Key.Number2:
                    ToolNote();
                    break;
                case Key.Number3:
                    ToolCamera();
                    break;
                case Key.BracketLeft:
                    if (e.ControlPressed) {
                        if (e.ShiftPressed) {
                            PlaybackDecreaseBeatDivisor();
                        } else {
                            PlaybackZoomIn();
                        }
                    }
                    break;
                case Key.BracketRight:
                    if (e.ControlPressed) {
                        if (e.ShiftPressed) {
                            PlaybackIncreaseBeatDivisor();
                        } else {
                            PlaybackZoomOut();
                        }
                    }
                    break;
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
                case Key.Up:
                    PlaybackIncreaseRate();
                    break;
                case Key.Down:
                    PlaybackDecreaseRate();
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
            Track.Seek(0);
            Story.ClearActives();
        }

        public void Seek(double time) {
            Track.Seek(time);
            Story.ClearActives();
        }

        private void ProjectPlay() {
            ProjectSave();
            this.Push(new PlayScreen());
        }

        private void ProjectRefresh() {
            ProjectSave();
            Story.Open(@"../../../story.json", true);
            Seek(Story.GetEditorSettings().TrackTime);
        }

        private void ProjectSave() {
            Play(false);
            Story.GetEditorSettings().TrackTime = Track.CurrentTime;
            Story.GetEditorSettings().TrackPlaybackRate = Track.Tempo.Value;
            Story.GetEditorSettings().SnapDivisor = SnapDivisor;
            Story.GetEditorSettings().BeatSnapDivisorIndex = NotesTimeline.DivisorIndex;
            Story.Save(@"../../../story.json");
        }

        private void EditUndo() => Reversibles.Undo();

        private void EditRedo() => Reversibles.Redo();

        private void ViewCommandPanel() {
            if (IsCommandPanelVisible) {
                CommandPanel.Hide();
            } else {
                CommandPanel.Show();
            }
            IsCommandPanelVisible = !IsCommandPanelVisible;
        }

        private void PlaybackPlay() {
            if (Track.CurrentTime < Track.Length) {
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

        private void PlaybackSetRate(double rate = 1.0) => Track.TempoTo(Math.Clamp(rate, 0.1, 1));

        private void PlaybackIncreaseRate() => PlaybackIncreaseRate(0.1);

        private void PlaybackDecreaseRate() => PlaybackDecreaseRate(0.1);

        public void PlaybackIncreaseRate(double step = 0.1) => PlaybackSetRate(Math.Clamp(Track.Tempo.Value + step, 0.1, 1));

        public void PlaybackDecreaseRate(double step = 0.1) => PlaybackSetRate(Math.Clamp(Track.Tempo.Value - step, 0.1, 1));

        private void VolumeIncrease() => VolumeIncrease(0.1);

        private void VolumeDecrease() => VolumeDecrease(0.1);

        // Note that Volume is set in a special framework.ini that is unique to your computer,
        // for example my path is: C:\Users\Wax Chug da Gwad\AppData\Roaming\S2VX\framework.ini
        private void VolumeSet(double volume = 1.0) => Audio.Volume.Value = volume;

        public void VolumeIncrease(double step = 0.1) => VolumeSet(Audio.Volume.Value + step);

        public void VolumeDecrease(double step = 0.1) => VolumeSet(Audio.Volume.Value - step);

        public void SnapDivisorIncrease() {
            if (SnapDivisor == MaxSnapDivisor) {
                // From most number of snap points to Free
                SnapDivisor = 0;
            } else {
                SnapDivisor *= 2;
            }
        }

        public void SnapDivisorDecrease() {
            switch (SnapDivisor) {
                case 0:
                    // From Free to most number of snap points
                    SnapDivisor = MaxSnapDivisor;
                    break;
                case 1:
                    // Stay at least number of snap points
                    break;
                default:
                    SnapDivisor /= 2;
                    break;
            }
        }

        private void ToolSelect() => SetToolState(new SelectToolState());

        private void ToolNote() => SetToolState(new NoteToolState());

        private void ToolCamera() => SetToolState(new CameraToolState());

    }
}
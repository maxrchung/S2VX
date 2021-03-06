using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
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
        public const double TrackTimeTolerance = 0.03;  // ms away from a tick or end of track will be considered to be on that tick or end of track
        private const int MaxSnapDivisor = 16;

        public int SnapDivisor { get; private set; }
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;
        public ReversibleStack Reversibles { get; } = new ReversibleStack();
        public S2VXToolState ToolState { get; private set; } = new SelectToolState();

        [Resolved]
        private AudioManager Audio { get; set; }

        private S2VXStory Story { get; }
        public S2VXTrack Track { get; }
        private EditorUI EditorUI { get; set; }
        public Container<RelativeBox> NoteSelectionIndicators { get; } = new Container<RelativeBox> {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };
        private Container ToolContainer { get; set; } = new Container {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };
        public NotesTimeline NotesTimeline { get; } = new NotesTimeline();
        private Timeline Timeline { get; } = new Timeline();
        public CommandPanel CommandPanel { get; } = new CommandPanel();

        public EditorScreen(S2VXStory story, S2VXTrack track) {
            Story = story;
            Track = track;
        }

        // We need to explicitly cache dependencies like this so that we can
        // recache an EditorScreen whenever a new one is pushed
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var dependencies = new DependencyContainer(parent);
            dependencies.Cache(this);
            dependencies.Cache(Story);
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            LoadEditorSettings();
            SetTrackClock();

            ToolContainer.Child = ToolState;

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                Track,
                Story,
                EditorUI = CreateEditorUI()
            };
        }

        private void LoadEditorSettings() {
            var editorSettings = Story.EditorSettings;
            Seek(editorSettings.TrackTime);
            PlaybackSetRate(editorSettings.TrackPlaybackRate);
            SnapDivisor = editorSettings.SnapDivisor;
            NotesTimeline.DivisorIndex = editorSettings.BeatSnapDivisorIndex;
        }

        private EditorUI CreateEditorUI() {
            var editorUI = new EditorUI {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[] {
                        NoteSelectionIndicators,
                        ToolContainer,
                        NotesTimeline,
                        new EditorInfoBar(),
                        CreateMenu(),
                        Timeline,
                        CommandPanel
                    }
            };
            editorUI.State.Value = Visibility.Visible;
            return editorUI;
        }

        private BasicMenu CreateMenu() =>
            new(Direction.Horizontal, true) {
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
                            new MenuItem("Preview Gameplay (G)", ProjectPreview),
                            new MenuItem("Quit (Esc)", ProjectQuit),
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
                            new MenuItem("Editor Interface (Ctrl+H)", ViewEditorUI),
                            new MenuItem("Command Panel (C)", ViewCommandPanel),
                        }
                    },
                    new MenuItem("Playback")
                    {
                        Items = new[]
                        {
                            new MenuItem("Play/Pause (Space)", PlaybackPlay),
                            new MenuItem("Restart (Ctrl+Left)", PlaybackRestart),
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
                            new MenuItem("Hold Note (3)", ToolHoldNote),
                            new MenuItem("Camera (4)", ToolCamera),
                        }
                    }
                }
            };


        /// <summary>
        /// Sets the same clock for sections dependent on the Track
        /// </summary>
        private void SetTrackClock() {
            var trackClock = new FramedClock(Track);
            Story.Clock = trackClock;
            NotesTimeline.Clock = trackClock;
            Timeline.Clock = trackClock;
            ToolContainer.Clock = trackClock;
        }

        private void SetToolState(S2VXToolState newState) {
            ToolState.HandleExit();
            ToolState = newState;
            ToolContainer.Child = ToolState;
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
                case Key.Escape:
                    ProjectQuit();
                    break;
                case Key.C:
                    ViewCommandPanel();
                    break;
                case Key.G:
                    ProjectPreview();
                    break;
                case Key.H:
                    if (e.ControlPressed) {
                        ViewEditorUI();
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
                    ToolSelect();
                    break;
                case Key.Number2:
                    ToolNote();
                    break;
                case Key.Number3:
                    ToolHoldNote();
                    break;
                case Key.Number4:
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
                case Key.T:
                    PlaybackDisplay();
                    break;
                case Key.Left:
                    if (e.ControlPressed) {
                        PlaybackRestart();
                    } else {
                        NotesTimeline.SnapToTick(true);
                    }
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

        public void Restart() => Seek(0);

        public void Seek(double time) {
            // This check is done for two reasons:
            // 1. If Track.TrackLoaded is false, and therefore length is 0
            // 2. If the track's length is truly less than track time tolerance
            if (Track.Length >= TrackTimeTolerance) {
                time = Math.Clamp(time, 0, Track.Length - TrackTimeTolerance);
            }
            Track.Seek(time);
            Story.ClearActives();
        }

        private void ProjectPreview() {
            ProjectSave();
            var newStory = new S2VXStory(Story.StoryPath, false);
            var newTrack = S2VXTrack.Open(Track.AudioPath, Audio);
            this.Push(new PlayScreen(true, newStory, newTrack));
        }

        private void ProjectRefresh() {
            ProjectSave();
            try {
                Story.Open(Story.StoryPath, true);
            } catch (Exception e) {
                Console.WriteLine(e);
                this.Exit();
            }
            Seek(Story.EditorSettings.TrackTime);
        }

        private void ProjectSave() {
            Play(false);
            var editorSettings = Story.EditorSettings;
            editorSettings.TrackTime = Track.CurrentTime;
            editorSettings.TrackPlaybackRate = Track.Tempo.Value;
            editorSettings.SnapDivisor = SnapDivisor;
            editorSettings.BeatSnapDivisorIndex = NotesTimeline.DivisorIndex;
            Story.Save(Story.StoryPath);
        }

        private void ProjectQuit() => this.Push(new LeaveScreen());

        private void EditUndo() => Reversibles.Undo();

        private void EditRedo() => Reversibles.Redo();

        private void ViewEditorUI() => EditorUI.ToggleVisibility();

        private void ViewCommandPanel() => CommandPanel.ToggleVisibility();

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

        public void SnapDivisorDecrease() {
            if (SnapDivisor == MaxSnapDivisor) {
                // From most number of snap points to Free
                SnapDivisor = 0;
            } else {
                SnapDivisor *= 2;
            }
        }

        public void SnapDivisorIncrease() {
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

        private void ToolHoldNote() => SetToolState(new HoldNoteToolState());

        private void ToolCamera() => SetToolState(new CameraToolState());

    }
}

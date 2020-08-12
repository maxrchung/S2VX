using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
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

        private NotesTimeline NotesTimeline { get; } = new NotesTimeline();
        private Timeline Timeline { get; } = new Timeline();

        private ToolState ToolState { get; set; } = new SelectToolState();

        private Container ToolContainer { get; set; } = new Container {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            ToolContainer.Child = ToolState;
            InternalChildren = new Drawable[]
            {
                Story,
                ToolContainer,
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
                CommandPanel,
            };
        }

        protected override bool OnClick(ClickEvent e) => ToolState.OnToolClick(e);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "<Pending>")]
        protected override bool OnKeyDown(KeyDownEvent e) {
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
                case Key.Number2: {
                    ToolNote();
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

        private void ProjectRefresh() {
            Story.Save(@"../../../story.json");
            Story.Open(@"../../../story.json");
        }

        private void ProjectSave() => Story.Save(@"../../../story.json");

        private void ViewCommandPanel() {
            if (IsCommandPanelVisible) {
                CommandPanel.Hide();
            } else {
                CommandPanel.Show();
            }
            IsCommandPanelVisible = !IsCommandPanelVisible;
        }

        private void PlaybackPlay() => Story.Play(!Story.IsPlaying);

        private void PlaybackRestart() => Story.Restart();

        private void PlaybackDisplay() => Timeline.DisplayMS = !Timeline.DisplayMS;

        private void PlaybackSeekLeftTick() => NotesTimeline.SnapToTick(true);

        private void PlaybackSeekRightTick() => NotesTimeline.SnapToTick(false);

        private void PlaybackZoomOut() => NotesTimeline.HandleZoom(false);

        private void PlaybackZoomIn() => NotesTimeline.HandleZoom(true);

        private void PlaybackDecreaseBeatDivisor() => NotesTimeline.ChangeBeatDivisor(false);

        private void PlaybackIncreaseBeatDivisor() => NotesTimeline.ChangeBeatDivisor(true);

        private void ToolSelect() {
            ToolState = new SelectToolState();
            ToolContainer.Child = ToolState;
        }

        private void ToolNote() {
            ToolState = new NoteToolState();
            ToolContainer.Child = ToolState;
        }
    }
}

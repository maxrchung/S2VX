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
        private bool IsCommandPanelVisible { get; set; } = false;

        private Timeline Timeline { get; } = new Timeline();

        private ToolState ToolState { get; set; } = new SelectToolState();

        private Container ToolContainer { get; set; } = new Container {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            ToolContainer.Child = ToolState;
            InternalChildren = new Drawable[]
            {
                Story,
                ToolContainer,
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
                new NotesTimeline(),
                CommandPanel,
            };
        }

        protected override bool OnClick(ClickEvent e) => ToolState.OnToolClick(e);

        protected override bool OnMouseMove(MouseMoveEvent e) {
            var mousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            var relativePosition = (mousePosition - Story.DrawSize / 2) / Story.DrawWidth;
            var camera = Story.Camera;
            var rotatedPosition = Utils.Rotate(relativePosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var translatedPosition = scaledPosition + camera.Position;
            MousePosition = translatedPosition;
            return true;
        }

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
                        break;
                    }
                    ToolSelect();
                    break;
                }
                case Key.Number2: {
                    if (e.ControlPressed) {
                        ViewCommandPanel();
                        break;
                    }
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

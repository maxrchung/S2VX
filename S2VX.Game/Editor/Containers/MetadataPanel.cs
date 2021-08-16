using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story.Settings;

namespace S2VX.Game.Editor.Containers {
    public class MetadataPanel : OverlayContainer {
        private static Vector2 PanelSize { get; } = new(330, 230);
        private static Vector2 PanelPosition { get; } = new(0, S2VXGameBase.GameWidth / 2);
        private static Vector2 InputSize = new(200, 30);
        private const float Pad = 10;

        private string StoryDirectory { get; }

        public MetadataPanel(string storyDirectory) => StoryDirectory = storyDirectory;

        private FillFlowContainer Form { get; set; } = new() {
            Child = new SpriteText { Text = "Metadata Panel" },
            Direction = FillDirection.Vertical,
            Position = new(Pad),
            Size = PanelSize
        };

        [BackgroundDependencyLoader]
        private void Load() {
            var metadata = MetadataSettings.Load(StoryDirectory);
            var title = AddRow("Title", metadata.SongTitle);
            var artist = AddRow("Artist", metadata.SongArtist);
            var author = AddRow("Author", metadata.StoryAuthor);
            var description = AddRow("Description", metadata.MiscDescription);
            Form.Add(new BasicButton {
                Text = "Save",
                Action = () => {
                    metadata.SongTitle = title.Text;
                    metadata.SongArtist = artist.Text;
                    metadata.StoryAuthor = author.Text;
                    metadata.MiscDescription = description.Text;
                    metadata.Save(StoryDirectory);
                },
                Size = new(InputSize.X / 2, InputSize.Y)
            });

            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                Form
            };

            Origin = Anchor.CentreLeft;
            Position = PanelPosition;
            Size = PanelSize;
        }

        private BasicTextBox AddRow(string key, string value) {
            var keyContainer = new Container {
                Child = new SpriteText {
                    Anchor = Anchor.CentreRight,
                    Font = FontUsage.Default.With(weight: "Bold"),
                    Origin = Anchor.CentreRight,
                    Text = $"{key}:"
                },
                Size = new(InputSize.X / 2, InputSize.Y + Pad),
            };

            var textbox = new BasicTextBox {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = InputSize,
                Text = value.ToString(),
                X = Pad,
            };
            var valueContainer = new Container {
                Child = textbox,
                Size = new(InputSize.X, InputSize.Y + Pad),
            };

            var row = new FillFlowContainer() {
                Children = new Drawable[] {
                    keyContainer,
                    valueContainer
                },
                Size = new(PanelSize.X, InputSize.Y + Pad),
            };
            Form.Add(row);
            return textbox;
        }

        protected override void PopIn() => this.FadeIn(100);

        protected override void PopOut() => this.FadeOut(100);
    }
}

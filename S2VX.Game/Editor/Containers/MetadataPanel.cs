using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story;
using S2VX.Game.Story.Settings;
using System.IO;

namespace S2VX.Game.Editor.Containers {
    public class MetadataPanel : OverlayContainer {
        private static Vector2 PanelSize { get; } = new(320, 220);
        private static Vector2 InputSize = new(200, 30);
        private const float Pad = 10;

        [Resolved]
        private S2VXStory Story { get; set; }
        public int BackgroundDependencyLoader { get; }

        private FillFlowContainer Form { get; set; } = new() {
            Direction = FillDirection.Vertical,
            Size = PanelSize,
            Child = new SpriteText { Text = "Metadata Panel" },
        };

        [BackgroundDependencyLoader]
        private void Load() {
            var storyDirectory = Path.GetDirectoryName(Story.StoryPath);
            var metadata = MetadataSettings.Load(storyDirectory);

            Size = PanelSize;
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
                    metadata.Save(storyDirectory);
                },
                Size = new(InputSize.X / 2, InputSize.Y)
            });

            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                Form
            };
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

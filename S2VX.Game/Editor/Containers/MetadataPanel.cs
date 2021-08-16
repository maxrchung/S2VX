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
        private static Vector2 PanelSize { get; } = new(500);

        [Resolved]
        private S2VXStory Story { get; set; }
        public int BackgroundDependencyLoader { get; }

        private FillFlowContainer Container { get; set; } = new() {
            Direction = FillDirection.Vertical,
            Size = new(500)
        };

        [BackgroundDependencyLoader]
        private void Load() {
            var storyDirectory = Path.GetDirectoryName(Story.StoryPath);
            var metadata = MetadataSettings.Load(storyDirectory);

            Size = PanelSize;
            AddRow("Score", metadata.SongTitle);
            AddRow("Artist", metadata.SongArtist);
            AddRow("Author", metadata.StoryAuthor);
            AddRow("Description", metadata.MiscDescription);
            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                Container
            };
        }

        private void AddRow(string key, string value) {
            var keyDisplay = new SpriteText {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Font = FontUsage.Default.With(weight: "Bold"),
                Text = $"{key}:"
            };
            var valueDisplay = new BasicTextBox {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                X = 10,
                Size = new(200, 30),
                Text = value.ToString()
            };
            var row = new FillFlowContainer() {
                Size = new(300, 50),
                Children = new Drawable[] {
                    keyDisplay,
                    valueDisplay
                }
            };
            Container.Add(row);
        }

        protected override void PopIn() => this.FadeIn(100);

        protected override void PopOut() => this.FadeOut(100);
    }
}

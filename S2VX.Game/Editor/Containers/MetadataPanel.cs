﻿using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story.Settings;

namespace S2VX.Game.Editor.Containers {
    public class MetadataPanel : S2VXOverlayContainer {
        private static Vector2 PanelSize { get; } = new(330, 240);
        private static Vector2 PanelPosition = new(0, S2VXGameBase.GameWidth / 2);
        private static Vector2 InputSize = new(200, 30);
        private const float Pad = 10;

        private string StoryDirectory { get; }

        public BasicTextBox TxtTitle { get; private set; }
        public BasicTextBox TxtArtist { get; private set; }
        public BasicTextBox TxtAuthor { get; private set; }
        public BasicTextBox TxtDescription { get; private set; }
        public BasicButton BtnSave { get; private set; }

        public MetadataPanel(string storyDirectory) => StoryDirectory = storyDirectory;

        private FillFlowContainer Form { get; set; } = new() {
            Direction = FillDirection.Vertical,
            Padding = new(Pad),
            Size = PanelSize,
            Spacing = new(Pad),
            Child = new SpriteText { Text = "Metadata Panel" }
        };

        [BackgroundDependencyLoader]
        private void Load() {
            Origin = Anchor.CentreLeft;
            Position = PanelPosition;
            Size = PanelSize;

            var metadata = MetadataSettings.Load(StoryDirectory);
            TxtTitle = AddRow("Title", metadata.SongTitle);
            TxtArtist = AddRow("Artist", metadata.SongArtist);
            TxtAuthor = AddRow("Author", metadata.StoryAuthor);
            TxtDescription = AddRow("Description", metadata.MiscDescription);

            Form.Add(BtnSave = new BasicButton {
                Text = "Save",
                Action = () => {
                    metadata.SongTitle = TxtTitle.Text;
                    metadata.SongArtist = TxtArtist.Text;
                    metadata.StoryAuthor = TxtAuthor.Text;
                    metadata.MiscDescription = TxtDescription.Text;
                    metadata.Save();
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
                Size = new(InputSize.X / 2, InputSize.Y),
                Child = new SpriteText {
                    Anchor = Anchor.CentreRight,
                    Font = FontUsage.Default.With(weight: "Bold"),
                    Origin = Anchor.CentreRight,
                    Text = $"{key}:"
                }
            };

            var textbox = new BasicTextBox {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = InputSize,
                Text = value.ToString(),
            };
            var valueContainer = new Container {
                Size = new(InputSize.X, InputSize.Y),
                Child = textbox
            };

            var row = new FillFlowContainer() {
                Size = new(PanelSize.X, InputSize.Y),
                Spacing = new(Pad),
                Children = new Drawable[] {
                    keyContainer,
                    valueContainer
                }
            };
            Form.Add(row);
            return textbox;
        }
    }
}
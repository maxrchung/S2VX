using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace S2VX.Game.SongSelection.UserInterface {
    public class SelectedItemDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }
        [Resolved]
        private AudioManager Audio { get; set; }

        public static Vector2 DeleteButtonSize { get; } = new(106, 30);
        private static Vector2 ButtonSize { get; } = new(100, 30);

        private const float BoxSize = S2VXGameBase.GameWidth / 4;
        private const float TextSize = S2VXGameBase.GameWidth / 40;
        public List<SelectedItemDisplay> SelectionItems { get; }
        public string ItemName { get; set; }
        public string CurSelectionPath { get; set; }
        public Texture ThumbnailTexture { get; set; }
        public RelativeBox SelectedIndicatorBox { get; private set; }
        private IconButton Thumbnail { get; set; }
        private Container DeleteConfirmation { get; set; }
        //private void DeleteConfirmationFadeout() {

        //}
        private void DeleteStory() {
            Console.WriteLine(SelectionItems);
            SelectionItems.RemoveAll(s => s.ItemName == ItemName && s.CurSelectionPath == CurSelectionPath);
        }

        private static BasicButton CancelButton;
        //private static BasicButton OkButton;

        //private Container DeleteConfirmation { get; } = ;
        private IconButton DeleteButton { get; set; }

        public SelectedItemDisplay(Action loadSelectionScreen, List<SelectedItemDisplay> selectionItems, string itemName, string curSelectionPath,
            Texture thumbnailTexture = null) {
            SelectionItems = selectionItems;
            ItemName = itemName;
            CurSelectionPath = curSelectionPath;
            ThumbnailTexture = thumbnailTexture;
            DeleteConfirmation = new Container() {
                new Box {
                    Colour = Color4.Black,
                    Size = new Vector2(BoxSize)
                },
                new FillFlowContainer {
                    Children = new Drawable[] {
                        new SpriteText() {
                            Text = "Confirm delete?"
                        },
                        new BasicButton() {
                            Text = "OK",
                            Action = () => { DeleteStory(); loadSelectionScreen(); },
                            Size = ButtonSize,
                        },
                        CancelButton = new BasicButton() {
                            Text = "Cancel",
                            //Action = () => DeleteConfirmationFadeout,
                            Size = ButtonSize,
                        }
                    }
                }
            };
            CancelButton.Action = () => DeleteConfirmation.FadeOut();
            //OkButton.Action = () => Test();

        }

        protected override bool OnHover(HoverEvent e) {
            DeleteButton.FadeIn();
            SelectedIndicatorBox.Alpha = 1;
            Thumbnail.Alpha = 0.7f;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            DeleteButton.FadeOut(100);
            SelectedIndicatorBox.Alpha = 0;
            Thumbnail.Alpha = 1;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            DeleteConfirmation.FadeOut();
            //CancelButton.Action = () => DeleteConfirmation.FadeOut();
            //Console.WriteLine(CancelButton.Text);
            Width = BoxSize;
            Height = BoxSize;
            Margin = new MarginPadding {
                Vertical = BoxSize / 10,
                Horizontal = BoxSize / 10
            };

            InternalChildren = new Drawable[] {
                SelectedIndicatorBox = new RelativeBox {
                    Width = 1.1f,
                    Height = 1.1f,
                    Alpha = 0
                },
                Thumbnail = new IconButton {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(BoxSize),
                    Texture = ThumbnailTexture,
                    TextureName = "logo",
                    Action = () => {
                        Audio.Samples.Get("menuhit").Play();
                        Screens.Push(new SongSelectionScreen { CurSelectionPath = CurSelectionPath + "/" + ItemName });
                    },
                },
                // TextShadowBox
                new RelativeBox {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Height = 0.2f,
                    Colour = Color4.Black.Opacity(0.5f),
                },
                // TextContainer
                new TextFlowContainer(s => s.Font = new FontUsage("default", TextSize)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Y = 0.8f,
                    Height = 0.2f,
                    TextAnchor = Anchor.TopCentre,
                    Text = ItemName,
                },
                DeleteButton = new IconButton {
                    Action = () => DeleteConfirmation.FadeIn(),
                    Width = DeleteButtonSize.Y,
                    Height = DeleteButtonSize.Y,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Alpha = 0.7f,
                    //Colour = S2VXColorConstants.DarkYellow,
                    Icon = FontAwesome.Solid.Trash
                },
                DeleteConfirmation
            };
            DeleteButton.FadeOut();
        }

    }
}

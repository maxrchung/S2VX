using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor.UserInterface;

namespace S2VX.Game.Editor.Containers {
    public class TapPanel : S2VXOverlayContainer {
        private static Vector2 PanelSize { get; } = new(120, 250);
        private static Vector2 PanelPosition = new(0, S2VXGameBase.GameWidth / 2);
        private static Vector2 InputSize = new(100, 30);
        private const float Pad = 10;

        [BackgroundDependencyLoader]
        private void Load() {
            Origin = Anchor.CentreLeft;
            Position = PanelPosition;
            Size = PanelSize;

            var receptor = new TapReceptor();
            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new FillFlowContainer {
                    Padding = new(Pad),
                    Spacing = new(Pad),
                    Children = new Drawable[] {
                        new SpriteText {
                            Text = "Tap Panel"
                        },
                        receptor,
                        new SpriteText {
                            Current = receptor.TapsLabel
                        },
                        new SpriteText {
                            Current = receptor.BPMLabel
                        },
                        new BasicButton {
                            Action = () => receptor.Reset(),
                            Size = InputSize,
                            Text = "Reset"
                        }
                    }
                }
            };
        }
    }
}

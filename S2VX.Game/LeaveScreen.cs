using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace S2VX.Game {
    public class LeaveScreen : Screen {
        private Vector2 ButtonSize { get; set; } = new Vector2(100, 30);

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[] {
                new FillFlowContainer() {
                    new SpriteText() {
                    Text = "Are you sure you want to leave?"
                },
                    new BasicButton() {
                        Text = "OK",
                        Action = () => this.GetParentScreen().GetParentScreen().MakeCurrent(),
                        Size = ButtonSize
                    },
                    new BasicButton() {
                        Text = "Cancel",
                        Action = () => this.Exit(),
                        Size = ButtonSize
                    }
                }
            };
    }
}

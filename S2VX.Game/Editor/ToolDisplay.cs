using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game.Editor {
    public class ToolDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private SpriteText TxtTool { get; set; } = new SpriteText() {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 30, "500")
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Height = 0.03f;
            Width = 1.0f;
            Y = 0.075f;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                TxtTool
            };
        }

        protected override void Update() {
            TxtTool.Text = $"Tool: {Editor.ToolState.DisplayName()}";
            TxtTool.Font = TxtTool.Font.With(size: Editor.DrawWidth / 40);
        }
    }
}

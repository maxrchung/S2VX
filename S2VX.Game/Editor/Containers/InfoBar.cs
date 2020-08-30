using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using S2VX.Game.Editor.UserInterface;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game.Editor.Containers {
    public class InfoBar : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private SpriteText TxtTool { get; } = new SpriteText {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 30, "500")
        };

        private SpriteText TxtMousePosition { get; } = new SpriteText {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopCentre,
            Colour = Color4.White,
            Font = new FontUsage("default", 30, "500"),
        };

        private VolumeDisplay VolumeDisplay { get; } = new VolumeDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Width = 0.15f,
            TextAnchor = Anchor.TopCentre,
        };

        private void SetTextSize(float value) {
            TxtTool.Font = TxtTool.Font.With(size: value);
            TxtMousePosition.Font = TxtMousePosition.Font.With(size: value);
        }

        public const float InfoBarHeight = 0.03f;
        public const float InfoBarWidth = 1.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Height = InfoBarHeight;
            Width = InfoBarWidth;
            Y = NotesTimeline.TimelineHeight;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                TxtTool,
                TxtMousePosition,
                VolumeDisplay
            };
        }

        protected override void Update() {
            TxtTool.Text = $"Tool: {Editor.ToolState.DisplayName()}";
            TxtMousePosition.Text = S2VXUtils.Vector2ToString(Editor.MousePosition, 2);
            SetTextSize(Editor.DrawWidth / 40);
        }
    }
}

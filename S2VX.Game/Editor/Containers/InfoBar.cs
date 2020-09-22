using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using S2VX.Game.Editor.UserInterface;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game.Editor.Containers {
    public class InfoBar : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        private ToolDisplay ToolDisplay { get; } = new ToolDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Width = 0.4f,
            TextAnchor = Anchor.TopLeft,
        };

        private NoteSnapDivisorDisplay NoteSnapDivisorDisplay { get; } = new NoteSnapDivisorDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 0.4f,
            Width = 0.2f,
            TextAnchor = Anchor.TopCentre,
        };

        private MousePositionDisplay MousePositionDisplay { get; } = new MousePositionDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 0.6f,
            Width = 0.2f,
            TextAnchor = Anchor.TopCentre,
        };

        private VolumeDisplay VolumeDisplay { get; } = new VolumeDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 0.8f,
            Width = 0.2f,
            TextAnchor = Anchor.TopRight,
        };

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
                ToolDisplay,
                NoteSnapDivisorDisplay,
                MousePositionDisplay,
                VolumeDisplay
            };
        }
    }
}

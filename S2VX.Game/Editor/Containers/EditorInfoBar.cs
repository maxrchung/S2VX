using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using S2VX.Game.Editor.UserInterface;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game.Editor.Containers {
    public class EditorInfoBar : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        private ToolDisplay ToolDisplay { get; } = new() {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Width = 400,
            TextAnchor = Anchor.TopLeft,
        };
        public ApproachRateDisplay ApproachRateDisplay { get; } = new() {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 365,
            Width = 200,
            TextAnchor = Anchor.TopRight,
        };

        private NoteSnapDivisorDisplay NoteSnapDivisorDisplay { get; } = new() {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 565,
            Width = 200,
            TextAnchor = Anchor.TopCentre,
        };

        private MousePositionDisplay MousePositionDisplay { get; } = new() {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            X = 770,
            Width = 200,
            TextAnchor = Anchor.TopCentre,
        };

        private VolumeDisplay VolumeDisplay { get; } = new() {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
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
                VolumeDisplay,
                ApproachRateDisplay
            };

            UpdateVolumeDisplay();
        }

        public void UpdateVolumeDisplay() => VolumeDisplay.UpdateVolumeDisplay();
    }
}

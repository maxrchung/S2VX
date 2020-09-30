using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using S2VX.Game.Play.UserInterface;

namespace S2VX.Game.Play.Containers {
    public class PlayInfoBar : CompositeDrawable {

        public const float InfoBarHeight = 0.06f;
        public const float InfoBarWidth = 1.0f;

        private int HitErrorDisplayIndex;

        public void RecordHitError(int timingError) {
            var currHit = (HitErrorDisplay)HitErrorDisplays[HitErrorDisplayIndex++];
            currHit.IndicatorBox.FadeOut();
            if (HitErrorDisplayIndex == HitErrorDisplays.Children.Count) {
                HitErrorDisplayIndex = 0;
            }
            var nextHit = (HitErrorDisplay)HitErrorDisplays[HitErrorDisplayIndex];
            nextHit.IndicatorBox.FadeIn();
            currHit.UpdateHitError(timingError);
        }

        private FillFlowContainer HitErrorDisplays { get; set; } = new FillFlowContainer {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Children = new Drawable[] {
                new HitErrorDisplay{
                    IsInitiallySelected = true
                },
                new HitErrorDisplay{ },
                new HitErrorDisplay{ },
                new HitErrorDisplay{ },
                new HitErrorDisplay{ },
            }
        };

        private static HitErrorDisplay SelectedHitErrorDisplay() {
            var hitError = new HitErrorDisplay();
            hitError.IndicatorBox.Alpha = 1;
            return hitError;
        }

        [Cached]
        private ScoreDisplay ScoreDisplay { get; } = new ScoreDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Height = InfoBarHeight;
            Width = InfoBarWidth;

            InternalChildren = new Drawable[]
            {
                HitErrorDisplays,
                ScoreDisplay,
            };
        }
    }
}

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using S2VX.Game.Play.UserInterface;
using System.Collections.Generic;

namespace S2VX.Game.Play.Containers {
    public class HitErrorBar : CompositeDrawable {

        private int HitErrorDisplayIndex;
        private const int HitErrorDisplayCount = 10;

        public void RecordHitError(int timingError) {
            var currHit = (HitErrorDisplay)HitErrorDisplays[HitErrorDisplayIndex];
            currHit.IndicatorBox.FadeOut();

            HitErrorDisplayIndex = ++HitErrorDisplayIndex % HitErrorDisplays.Children.Count;
            var nextHit = (HitErrorDisplay)HitErrorDisplays[HitErrorDisplayIndex];
            nextHit.IndicatorBox.FadeIn();

            currHit.UpdateHitError(timingError);
        }

        private FillFlowContainer HitErrorDisplays { get; set; } = new FillFlowContainer {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Children = CreateHitErrorDisplays()
        };

        private static List<Drawable> CreateHitErrorDisplays() {
            var hitErrors = new List<Drawable> {
                new HitErrorDisplay {
                    IsInitiallySelected = true
                }
            };
            for (var i = 0; i < HitErrorDisplayCount - 1; ++i) {
                hitErrors.Add(new HitErrorDisplay { });
            }
            return hitErrors;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                HitErrorDisplays,
            };
        }
    }
}

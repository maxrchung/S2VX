using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class ScoreDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }

        [Resolved]
        private S2VXScore Score { get; set; }

        private TextFlowContainer TxtScore { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[]
            {
                TxtScore = new TextFlowContainer(s => s.Font = new FontUsage("default", Screens.DrawWidth / 20, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = TextAnchor,
                }
            };

        protected override void Update() => TxtScore.Text = Score.Value.ToString(CultureInfo.InvariantCulture);
    }
}

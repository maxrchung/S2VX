//using osu.Framework.Allocation;
//using osu.Framework.Graphics;
//using osu.Framework.Graphics.Containers;
//using S2VX.Game.Play.UserInterface;

//namespace S2VX.Game.Play.Containers {
//    public class ScoreInfoBar : CompositeDrawable {
//        public const float InfoBarHeight = 0.06f;
//        public const float InfoBarWidth = 1.0f;

//        [Resolved]
//        private ScoreInfo ScoreInfo { get; set; }

//        [BackgroundDependencyLoader]
//        private void Load() {
//            RelativeSizeAxes = Axes.Both;
//            RelativePositionAxes = Axes.Both;
//            Height = InfoBarHeight;
//            Width = InfoBarWidth;

//            InternalChildren = new Drawable[]
//            {
//                ScoreInfo,
//            };
//        }
//    }
//}

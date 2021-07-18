using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using System.Globalization;

namespace S2VX.Game {
    public class GlobalVolumeDisplay : CompositeDrawable {
        [Resolved]
        private AudioManager Audio { get; set; }
        public Anchor TextAnchor { get; set; }
        private TextFlowContainer Text { get; set; }

        private const float DisplayWidth = S2VXGameBase.GameWidth / 10;
        private const float DisplayHeight = S2VXGameBase.GameWidth / 20;
        private const int CornerPadding = 50;

        private Box VolumeBox { get; set; } = new Box {
            Width = DisplayWidth,
            Height = DisplayHeight,
            Colour = Color4.Black.Opacity(0.9f),
            Anchor = Anchor.BottomRight,
            Origin = Anchor.BottomRight,
            X = -CornerPadding,
            Y = -CornerPadding
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;

            //Width = S2VXGameBase.GameWidth / 20;
            //Height = S2VXGameBase.GameWidth / 20;
            //Colour = S2VXColorConstants.LightBlack;
            //Alpha = 1;
            //Anchor = Anchor.Centre;
            //Origin = Anchor.Centre;

            Text = new(s => s.Font = new("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                AutoSizeAxes = Axes.Both
            };

            InternalChildren = new Drawable[]
            {
                VolumeBox,
                Text
            };
        }

        // Note that Volume is set in a special framework.ini that is unique to your computer,
        // for example my path is: C:\Users\Wax Chug da Gwad\AppData\Roaming\S2VX\framework.ini
        private void VolumeSet(double volume = 1.0) => Audio.Volume.Value = volume;
        //EditorInfoBar.VolumeDisplay.UpdateDisplay();

        public void VolumeIncrease(double step = 0.1) => VolumeSet(Audio.Volume.Value + step);

        public void VolumeDecrease(double step = 0.1) => VolumeSet(Audio.Volume.Value - step);

        //public abstract void UpdateDisplay();

        //protected void UpdateDisplay(string text) => Text.Text = text;

        public void UpdateDisplay() => Text.Text = $"Volume: {Audio.Volume.Value.ToString("P0", CultureInfo.InvariantCulture)}";
    }
}

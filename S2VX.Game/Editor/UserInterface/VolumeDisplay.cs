using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor.UserInterface {
    public class VolumeDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        //[BackgroundDependencyLoader]
        //private void Load() {
        //    AutoSizeAxes = Axes.Both;
        //    AddInternal(TxtVolume = new(s => s.Font = new("default", SizeConsts.TextSize2, "500")) {
        //        TextAnchor = TextAnchor,
        //        Origin = Anchor.TopRight,
        //        Anchor = Anchor.TopRight,
        //        AutoSizeAxes = Axes.Both,
        //    });
        //}

        public override void UpdateDisplay() => UpdateDisplay($"Volume: {Audio.Volume.Value.ToString("P0", CultureInfo.InvariantCulture)}");

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.VolumeIncrease();
            } else {
                Editor.VolumeDecrease();
            }
            UpdateDisplay();
            return true;
        }
    }
}

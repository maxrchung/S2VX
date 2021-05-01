﻿using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor.UserInterface {
    public class VolumeDisplay : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        private TextFlowContainer TxtVolume { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;
            AddInternal(TxtVolume = new TextFlowContainer(s => s.Font = new FontUsage("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                Origin = Anchor.TopRight,
                Anchor = Anchor.TopRight,
                AutoSizeAxes = Axes.Both,
            });
        }

        protected override void Update() => TxtVolume.Text = $"Volume: {Audio.Volume.Value.ToString("P0", CultureInfo.InvariantCulture)}";

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.VolumeIncrease();
            } else {
                Editor.VolumeDecrease();
            }
            return true;
        }
    }
}

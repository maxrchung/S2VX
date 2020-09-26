using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {
        [Resolved]
        private AudioManager Audio { get; set; }

        protected override bool OnClick(ClickEvent e) {
            var miss = Audio.Samples.Get("miss");
            miss.Play();

            Console.WriteLine("GameNote clicked");
            return false;
        }
    }
}

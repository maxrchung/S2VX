using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class EditorNote : S2VXNote {
        [Resolved]
        private AudioManager Audio { get; set; }

        protected override bool OnClick(ClickEvent e) {
            var hit = Audio.Samples.Get("hit");
            hit.Play();

            Console.WriteLine("EditorNote clicked");
            return false;
        }
    }
}

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {
        private SampleChannel Miss { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) =>
            Miss = audio.Samples.Get("miss");

        protected override bool OnClick(ClickEvent e) {
            Miss.Play();

            Console.WriteLine("GameNote clicked");
            return false;
        }
    }
}

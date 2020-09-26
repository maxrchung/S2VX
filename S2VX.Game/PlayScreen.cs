using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Story;

namespace S2VX.Game {
    public class PlayScreen : Screen {
        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var story = new S2VXStory();
            story.Open(@"../../../story.json", false);
            story.ClearActives();

            var track = new DrawableTrack(audio.Tracks.Get(@"Camellia_MEGALOVANIA_Remix.mp3"));
            var settings = story.GetEditorSettings();
            track.Tempo.Value = settings.TrackPlaybackRate;
            track.Seek(settings.TrackTime);
            track.Start();
            Clock = new FramedClock(track);
            InternalChildren = new Drawable[] {
                story,
                track
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    this.Exit();
                    return true;
            }
            return false;
        }
    }
}

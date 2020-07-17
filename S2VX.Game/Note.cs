using osu.Framework.Allocation;
using osu.Framework.Utils;
using osuTK;

namespace S2VX.Game
{
    public class Note : RelativeBox
    {
        public float EndTime { get; set; } = 0;
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        private Story story { get; set; } = new Story();
        private Camera camera { get; set; } = new Camera();
        private Notes notes { get; set; } = new Notes();

        [BackgroundDependencyLoader]
        private void load(Story story, Camera camera, Notes notes)
        {
            this.story = story;
            this.camera = camera;
            this.notes = notes;
            Alpha = 0;
            AlwaysPresent = true;
        }

        protected override void Update()
        {
            var time = story.GameTime;
            var endFadeOut = EndTime + notes.FadeOutTime;

            if (time >= endFadeOut)
            {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            Rotation = camera.Rotation;
            Size = camera.Scale;
            Position = Utils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;

            var startTime = EndTime - notes.ShowTime;
            if (time >= EndTime)
            {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
                Alpha = alpha;
            }
            else if (time >= startTime)
            {
                Alpha = 1;
            }
            else
            {
                var startFadeIn = startTime - notes.FadeInTime;
                var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                Alpha = alpha;
            }
        }
    }
}

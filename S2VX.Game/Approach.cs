using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;

namespace S2VX.Game
{
    public class Approach : CompositeDrawable
    {
        public float EndTime = 0;
        public Vector2 Coordinates = Vector2.Zero;

        private Camera camera = new Camera();
        private Notes notes = new Notes();

        private RelativeBox[] lines = new RelativeBox[]
        {
            new RelativeBox(),
            new RelativeBox(),
            new RelativeBox(),
            new RelativeBox()
        };

        [BackgroundDependencyLoader]
        private void load(Camera camera, Notes notes)
        {
            this.camera = camera;
            this.notes = notes;
            Alpha = 0;
            AlwaysPresent = true;
        }

        protected override void Update()
        {
            var time = Time.Current;
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

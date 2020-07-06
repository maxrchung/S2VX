using osu.Framework.Allocation;
using osuTK;

namespace S2VX.Game
{
    public class Note : RelativeBox
    {
        public float EndTime = 0;
        public Vector2 Coordinates = Vector2.Zero;

        private Camera camera = new Camera();
        private Notes notes = new Notes();

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
            Rotation = camera.Rotation;
            Size = camera.Scale;
            Position = Utils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;

            var time = Time.Current;
            var startTime = EndTime - notes.ShowTime;
            if (time >= startTime)
            {
                Alpha = 1;
            }

            if (time >= EndTime)
            {
                Alpha = 0;
            }
        }
    }
}

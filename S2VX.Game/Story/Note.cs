using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;

namespace S2VX.Game.Story {
    public class Note : CompositeDrawable {
        public double EndTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private RelativeBox[] ApproachLines { get; set; } = new RelativeBox[4]
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox() // left
        };

        public RelativeBox SquareNote { get; private set; } = new RelativeBox();

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            Alpha = 0;
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[] {
                SquareNote,
                ApproachLines[0],
                ApproachLines[1],
                ApproachLines[2],
                ApproachLines[3],
            };
        }

        protected override void Update() {
            var notes = Story.Notes;
            var camera = Story.Camera;

            var time = Time.Current;
            var endFadeOut = EndTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }
            var startTime = EndTime - notes.ShowTime;
            var startFadeIn = startTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = notes.ApproachThickness;

            SquareNote.Rotation = rotation;
            SquareNote.Size = scale;
            SquareNote.Position = S2VXUtils.Rotate(Coordinates - position, SquareNote.Rotation) * SquareNote.Size.X;


            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            var offset = S2VXUtils.Rotate(Coordinates - position, rotation) * scale;

            var distance = time < EndTime
                ? Interpolation.ValueAt(time, notes.ApproachDistance, scale.X / 2, startFadeIn, EndTime)
                : scale.X / 2;
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            ApproachLines[0].Position = offset + rotationY;
            ApproachLines[0].Rotation = rotation;
            ApproachLines[0].Size = new Vector2(overlap, thickness);
            ApproachLines[1].Position = offset - rotationY;
            ApproachLines[1].Rotation = rotation;
            ApproachLines[1].Size = new Vector2(overlap, thickness);
            ApproachLines[2].Position = offset + rotationX;
            ApproachLines[2].Rotation = rotation;
            ApproachLines[2].Size = new Vector2(thickness, overlap);
            ApproachLines[3].Position = offset - rotationX;
            ApproachLines[3].Rotation = rotation;
            ApproachLines[3].Size = new Vector2(thickness, overlap);

            if (time >= EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
                Alpha = alpha;
            } else if (time >= startTime) {
                Alpha = 1;
            } else {
                var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                Alpha = alpha;
            }
        }
    }
}

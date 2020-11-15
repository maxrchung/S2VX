using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace S2VX.Game.Story.Note {
    public abstract class S2VXNote : CompositeDrawable, IComparable<S2VXNote> {
        public double HitTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        public Approach Approach { get; set; }

        private RelativeBox BoxOuter { get; } = new RelativeBox();
        private RelativeBox BoxInner { get; } = new RelativeBox();

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Alpha = 1;
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[] {
                BoxOuter,
                BoxInner
            };
        }

        // These Update setters modify both the Note and a corresponding Approach
        public virtual void UpdateHitTime(double hitTime) {
            Approach.HitTime = hitTime;
            HitTime = hitTime;
            Story.Notes.Sort();
        }

        public void UpdateCoordinates(Vector2 coordinates) {
            Approach.Coordinates = coordinates;
            Coordinates = coordinates;
        }

        public void UpdatePlacement() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var grid = Story.Grid;

            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(notes.OutlineThickness);

            Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;
            BoxOuter.Colour = notes.OutlineColor;
        }

        // Sort Notes from highest end time to lowest end time
        public int CompareTo(S2VXNote other) => other.HitTime.CompareTo(HitTime);
    }
}

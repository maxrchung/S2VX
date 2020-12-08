using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor;
using System;

namespace S2VX.Game.Story.Note {
    public abstract class S2VXNote : CompositeDrawable, IComparable<S2VXNote> {
        public double HitTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;
        public Approach Approach { get; set; }
        public float OutlineThickness { get; set; }
        public Color4 OutlineColor { get; set; }

        protected RelativeBox BoxOuter { get; } = new RelativeBox();
        protected RelativeBox BoxInner { get; } = new RelativeBox();

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

        // These Update setters modify the Note, the corresponding Approach, and the selection in NotesTimeline
        public virtual void UpdateHitTime(double hitTime) {
            Approach.HitTime = hitTime;
            HitTime = hitTime;
            Story.Notes.Sort();
        }

        public virtual void UpdateCoordinates(Vector2 coordinates) {
            Approach.Coordinates = coordinates;
            Coordinates = coordinates;
        }

        /// <summary>
        /// Main entrypoint for a note's Update functionality
        /// </summary>
        /// <returns> Returns if this note should be removed.</returns>
        public abstract bool UpdateNote();

        /// <summary>
        /// Updates a note's color/alpha
        /// </summary>
        protected abstract void UpdateColor();

        /// <summary>
        /// Updates a note's position/rotation/size
        /// </summary>
        protected virtual void UpdatePosition() {
            var camera = Story.Camera;
            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(Story.Grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(OutlineThickness);

            Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;
        }

        public Color4 GetColor() => BoxInner.Colour;

        public void SetColor(Color4 color) => BoxInner.Colour = color;

        public void SetOutlineColor(Color4 color) => BoxOuter.Colour = color;

        public void SetAlpha(float alpha) => BoxInner.Alpha = alpha;

        // Sort Notes from highest end time to lowest end time
        public int CompareTo(S2VXNote other) => other.HitTime.CompareTo(HitTime);

        /// <summary>
        /// Pushes a Reversible to the Editor Reversibles stack
        /// </summary>
        public virtual void ReversibleRemove(S2VXStory story, EditorScreen editor) { }
    }
}

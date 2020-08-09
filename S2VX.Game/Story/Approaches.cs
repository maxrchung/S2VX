using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System.Collections.Generic;

namespace S2VX.Game.Story {
    public class Approaches : CompositeDrawable {
        public List<Approach> Children = new List<Approach>();
        public float Distance { get; set; } = 0.5f;
        public float Thickness { get; set; } = 0.005f;

        public void AddApproach(Vector2 position, double time) {
            var approach = new Approach {
                Coordinates = position,
                EndTime = time
            };
            Children.Add(approach);
            AddInternal(approach);
        }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Children;
        }

        protected override void Update() {
            var notes = Story.Notes;
            Alpha = notes.Alpha;
            Colour = notes.Colour;
        }
    }
}

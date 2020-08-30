using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story {
    public class Approaches : CompositeDrawable {
        public List<Approach> Children { get; private set; } = new List<Approach>();
        public void SetChildren(List<Approach> approaches) => Children = approaches;

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

        public void RemoveApproach(Note note) {
            var approach = Children.Where(a =>
                a.Coordinates == note.Coordinates &&
                a.EndTime == note.EndTime
            ).First();
            Children.Remove(approach);
            RemoveInternal(approach);
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

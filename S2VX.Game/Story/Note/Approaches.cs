using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class Approaches : CompositeDrawable {
        public List<Approach> Children { get; private set; } = new List<Approach>();
        public void SetChildren(List<Approach> approaches) {
            Children = approaches;
            InternalChildren = Children;
        }

        public float Distance { get; set; }
        public float Thickness { get; set; }

        public Approach AddApproach(S2VXNote note) {
            var approach = new Approach {
                Coordinates = note.Coordinates,
                HitTime = note.HitTime
            };
            Children.Add(approach);
            AddInternal(approach);
            return approach;
        }

        public HoldApproach AddHoldApproach(HoldNote note) {
            var approach = new HoldApproach {
                Coordinates = note.Coordinates,
                HitTime = note.HitTime,
                EndTime = note.EndTime,
                EndCoordinates = note.EndCoordinates
            };
            Children.Add(approach);
            AddInternal(approach);
            return approach;
        }

        public void RemoveApproach(S2VXNote note) {
            var approach = note.Approach;
            Children.Remove(approach);
            RemoveInternal(approach);
        }

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;

        protected override void Update() => Children.ForEach(approach => {
            // There is a visual glitch where you can sometimes see a slight
            // flicker if Thickness is set to 0. We can properly hide this by
            // adjusting the alpha and also do some optimization by skipping the
            // Update.
            if (Thickness == 0) {
                approach.Alpha = 0;
            } else {
                approach.UpdateApproach();
            }
        });
    }
}

﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;

namespace S2VX.Game.Story {
    public class Approaches : CompositeDrawable {
        public List<Approach> Children { get; private set; } = new List<Approach>();
        public void SetChildren(List<Approach> approaches) => Children = approaches;

        public float Distance { get; set; } = 0.5f;
        public float Thickness { get; set; } = 0.005f;

        public Approach AddApproach(Note note) {
            var approach = new Approach {
                Coordinates = note.Coordinates,
                EndTime = note.EndTime
            };
            Children.Add(approach);
            AddInternal(approach);
            return approach;
        }

        public void RemoveApproach(Note note) {
            var approach = note.Approach;
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

﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;
using System;

namespace S2VX.Game.Story.Note {
    public abstract class S2VXNote : CompositeDrawable, IComparable<S2VXNote> {
        public double HitTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        public Approach Approach { get; set; }

        protected RelativeBox BoxOuter { get; } = new RelativeBox();
        protected RelativeBox BoxInner { get; } = new RelativeBox();

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Alpha = 0;
            AlwaysPresent = true;
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

        protected void UpdatePlacement() {
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

        protected override void Update() {
            var notes = Story.Notes;
            var time = Time.Current;
            var endFadeOut = HitTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            UpdatePlacement();

            var startTime = HitTime - notes.ShowTime;
            if (time >= HitTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, HitTime, endFadeOut);
                Alpha = alpha;
            } else if (time >= startTime) {
                Alpha = 1;
            } else {
                var startFadeIn = startTime - notes.FadeInTime;
                var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                Alpha = alpha;
            }
        }

        // Sort Notes from highest end time to lowest end time
        public int CompareTo(S2VXNote other) => other.HitTime.CompareTo(HitTime);
    }
}

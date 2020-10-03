﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using osu.Framework.Allocation;
using osu.Framework.Utils;
using osuTK;
using System;

namespace S2VX.Game.Story.Note {
    public abstract class S2VXNote : RelativeBox, IComparable<S2VXNote> {
        public double EndTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        public Approach Approach { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Alpha = 0;
            AlwaysPresent = true;
        }

        // These Update setters modify both the Note and a corresponding Approach
        public void UpdateEndTime(double endTime) {
            Approach.EndTime = endTime;
            EndTime = endTime;
        }

        public void UpdateCoordinates(Vector2 coordinates) {
            Approach.Coordinates = coordinates;
            Coordinates = coordinates;
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

            Rotation = camera.Rotation;
            Size = camera.Scale;
            Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;

            var startTime = EndTime - notes.ShowTime;
            if (time >= EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
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
        public int CompareTo(S2VXNote other) => other.EndTime.CompareTo(EndTime);
    }
}

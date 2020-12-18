using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNote : HoldNote {
        private int NumHitSounds { get; set; }
        private List<double> HitSoundTimes { get; set; }
        private SampleChannel Hit { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private ScreenStack Screens { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            HitSoundTimes = new List<double>() { HitTime, EndTime };
            AddInternal(AnchorPath);
            //AddInternal(HeadAnchor);
            AddInternal(TailAnchor);
        }

        private Path AnchorPath { get; set; } = new Path() {
            Anchor = Anchor.Centre
        };

        private Box HeadAnchor { get; set; } = new Box() {
            Colour = S2VXColorConstants.BrickRed,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };

        private Box TailAnchor { get; set; } = new Box() {
            Colour = S2VXColorConstants.BrickRed,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };

        public override void UpdateHitTime(double hitTime) {
            EndTime = hitTime + EndTime - HitTime;
            base.UpdateHitTime(hitTime);
            HoldApproach.EndTime = EndTime;
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public override void UpdateEndTime(double endTime) {
            if (endTime < HitTime) {
                return;
            }
            EndTime = endTime;
            HoldApproach.EndTime = EndTime;
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public override void UpdateCoordinates(Vector2 coordinates) {
            // Prevent changes at end time or else we'll get some wild interpolation
            if (EndTime - Time.Current <= EditorScreen.TrackTimeTolerance) {
                return;
            }

            var startCoordinates = Interpolation.ValueAt(HitTime, coordinates, EndCoordinates, Time.Current, EndTime);
            base.UpdateCoordinates(startCoordinates);
            EndCoordinates = startCoordinates + EndCoordinates - Coordinates;
            HoldApproach.EndCoordinates = EndCoordinates;
            // CHANGE tail anchor point here
        }

        public override bool UpdateNote() {
            UpdateColor();
            UpdatePosition();

            var time = Time.Current;
            // Deduct number of hit sounds to play once we've passed each HitSoundTime
            if (NumHitSounds > 0 && time >= HitSoundTimes[^NumHitSounds]) {
                --NumHitSounds;
                Hit.Play();
            }

            // Reset hit sound counter if clock is running and before timing points
            if (Clock.IsRunning) {
                NumHitSounds = HitSoundTimes.Count - GetNumTimingPointsPassed();
            }

            HeadAnchor.Size = Size;
            TailAnchor.Size = Size;
            var startCoordinates = Interpolation.ValueAt(HitTime, Coordinates, EndCoordinates, Time.Current, EndTime);
            //EndCoordinates = startCoordinates + EndCoordinates - Coordinates; assuming this is actual coordinates
            //realtive to the editorscreen

            var camera = Story.Camera;
            AnchorPath.PathRadius = OutlineThickness * Screens.DrawWidth / 2;
            var noteWidth = camera.Scale.X * Screens.DrawWidth;

            var clampedTime = Math.Clamp(time, HitTime, EndTime);
            var currCoordinates = Interpolation.ValueAt(clampedTime, Coordinates, EndCoordinates, HitTime, EndTime);
            var endPosition = (EndCoordinates - currCoordinates) * noteWidth;

            var vertices = new List<Vector2>() {
                new Vector2(0, 0),
                endPosition,
            };

            //TailAnchor.Position = endPosition;
            TailAnchor.Position = new Vector2(1, 1);
            AnchorPath.Vertices = vertices;
            AnchorPath.Position = -AnchorPath.PositionInBoundingBox(Vector2.Zero);
            return false;
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time (to Hit time) to End time
            else if (time < EndTime) {
                Alpha = 1;
            }
            // End time to Fade out time
            else if (time < EndTime + notes.FadeOutTime) {
                var startTime = EndTime;
                var endTime = EndTime + notes.FadeOutTime;
                Alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        private int GetNumTimingPointsPassed() {
            var time = Time.Current;
            var ans = 0;
            foreach (var hitSoundTime in HitSoundTimes) {
                if (time >= hitSoundTime) {
                    ++ans;
                } else {
                    break;
                }
            }
            return ans;
        }

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveHoldNote(story, this, editor));
    }
}

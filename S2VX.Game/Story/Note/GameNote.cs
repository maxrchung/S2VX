using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {
        private SampleChannel Hit { get; set; }
        private SampleChannel Miss { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            Miss = audio.Samples.Get("miss");
        }

        [Resolved]
        private ScoreInfo ScoreInfo { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private ScreenStack ScreenStack { get; set; }

        private const int MissThreshold = 200;
        private int TimingError;

        private static bool BlockClicking { get; set; }

        private void Delete() {
            var playScreen = (PlayScreen)ScreenStack.CurrentScreen;
            playScreen.PlayInfoBar.RecordHitError(TimingError);
            if (Math.Abs(TimingError) < MissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }

            Story.RemoveNote(this);
        }

        private void RecordMiss() {
            var missThreshold = MissThreshold;
            TimingError = missThreshold;
            ScoreInfo.AddScore(missThreshold);
            Delete();
        }

        // Notes are clickable if they are visible on screen, not missed, and is the earliest note
        private bool IsClickable() {
            var time = Time.Current;
            // Limit timing error to be +/- MissThreshold (though it will never be >= MissThreshold since RecordMiss would have already run)
            TimingError = (int)Math.Round(Math.Clamp(time - EndTime, -MissThreshold, MissThreshold));
            var inMissThreshold = TimingError <= MissThreshold && Alpha > 0;
            var earliestNote = (GameNote)Story.Notes.Children[^1];
            var isEarliestNote = earliestNote == this;
            return inMissThreshold && isEarliestNote;
        }

        private void ClickNote() {
            ScoreInfo.AddScore(Math.Abs(TimingError));
            Delete();
            BlockClicking = true;
        }

        // Unblocks clicking on the last call of OnMouseDown / OnKeyDown
        private void CheckAndResetClicking() {
            if (Story.Notes.Children.Count > 0 && this == Story.Notes.Children[0]) {
                BlockClicking = false;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable() && !BlockClicking) {
                ClickNote();
            }

            CheckAndResetClicking();
            return false;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (IsClickable() && IsHovered && !BlockClicking) {
                switch (e.Key) {
                    case Key.Z:
                        ClickNote();
                        break;
                    case Key.X:
                        ClickNote();
                        break;
                    case Key.C:
                        ClickNote();
                        break;
                    case Key.V:
                        ClickNote();
                        break;
                    case Key.A:
                        ClickNote();
                        break;
                    case Key.S:
                        ClickNote();
                        break;
                    case Key.D:
                        ClickNote();
                        break;
                    case Key.F:
                        ClickNote();
                        break;
                    default:
                        break;
                }
            }

            CheckAndResetClicking();
            return false;
        }

        protected override void Update() {
            base.Update();
            var time = Time.Current;
            if (time >= EndTime) {
                // Hold the note at fully visible until after MissThreshold
                var notes = Story.Notes;
                var startFadeTime = EndTime + MissThreshold;
                var endFadeTime = startFadeTime + notes.FadeOutTime;
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startFadeTime, endFadeTime);
                Alpha = alpha;
                Colour = Color4.Red;
                if (time >= EndTime + MissThreshold) {
                    RecordMiss();
                }
            }
        }
    }
}

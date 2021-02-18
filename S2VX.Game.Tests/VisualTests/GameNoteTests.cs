﻿using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests {
    public class GameNoteTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        private S2VXStory Story { get; set; } = new S2VXStory();
        private StopwatchClock Stopwatch { get; set; }
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("VisualTests", "TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXUtils.LoadDrawableTrack(audioPath, Audio))));
        }

        [SetUp]
        public new void SetUp() => Schedule(() => {
            PlayScreen.PlayInfoBar.ScoreInfo.ClearScore();
            Story.Reset();
            Story.Clock = new FramedClock(Stopwatch = new StopwatchClock());
        });

        [Test]
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            AddStep("Add notes", () => {
                for (var i = 0; i < 1000; i += 50) {
                    Story.AddNote(new GameNote { HitTime = i });
                }
            });
            AddStep("Move mouse", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until all notes are hit", () => Story.Notes.Children.Count == 0);
            AddAssert("Does not trigger multiple notes", () =>
                Story.Notes.Children.Count - 1 * GameNote.MissThreshold == PlayScreen.PlayInfoBar.ScoreInfo.Score
            );
        }

    }
}
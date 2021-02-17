using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Timing;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class GameNoteTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        private S2VXStory Story { get; set; }
        private StopwatchClock StoryClock { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Story.Clock = new FramedClock(StoryClock = new StopwatchClock());
            Add(new PlayScreen(
                false,
                Story = new S2VXStory(),
                S2VXUtils.LoadDrawableTrack("", Audio))
            );
        }

        [Test]
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            var firstNote = 
            AddStep("Add notes", () => {
                for (var i = 0; i < 100; i += 10) {
                    Story.AddNote(new GameNote { HitTime = i });
                }
            });
            AddStep("Move mouse", )
        }

    }
}

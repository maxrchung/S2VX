using NUnit.Framework;
using osu.Framework.Allocation;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class GameNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();

        [BackgroundDependencyLoader]
        private void Load() =>
            Add(Story);

        [Test]
        public void OnClick_StackedNotes_HitsTopNote() {
            GameNote gameNote1 = null;
            AddStep("Add game note", () => Story.AddNote(gameNote1 = new GameNote {
                HitTime = Time.Current
            }));
            AddStep("Add game note", () => Story.AddNote(new GameNote {
                HitTime = Time.Current + 1
            }));
            AddStep("Add game note", () => Story.AddNote(new GameNote {
                HitTime = Time.Current + 2
            }));
            AddStep("move mouse to centre", () => InputManager.MoveMouseTo(gameNote1.Position));
        }
    }
}

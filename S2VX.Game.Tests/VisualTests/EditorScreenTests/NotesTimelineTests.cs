using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Utils;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.VisualTests.EditorScreenTests {
    public class NotesTimelineTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }
        private S2VXTrack Track { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var storyPath = Path.Combine("VisualTests", "EditorScreenTests", "ValidStory.s2ry");
            var story = new S2VXStory();
            story.Open(storyPath, true);

            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Track = S2VXTrack.Open(audioPath, audio);

            Add(new ScreenStack(Editor = new EditorScreen(story, Track)));
        }

        [Test]
        public void SnapToTick_AfterTimingChange_SnapsToRightTick() {
            AddStep("Seek after timing change", () => Editor.Seek(100));
            AddStep("Snap to the right", () => Editor.NotesTimeline.SnapToTick(false));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 350));
        }

        [Test]
        public void SnapToTick_AfterTimingChange_SnapsToLeftTick() {
            AddStep("Seek after timing change", () => Editor.Seek(300));
            AddStep("Snap to the left", () => Editor.NotesTimeline.SnapToTick(true));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 100));
        }

        [Test]
        public void SnapToTick_TransitionTimingChange_SnapsToTimeBasedOnCurrentBPM() {
            AddStep("Snap to the right", () => Editor.NotesTimeline.SnapToTick(false));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 125));
        }
    }
}

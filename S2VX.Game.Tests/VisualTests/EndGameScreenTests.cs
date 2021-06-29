using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.EndGame;
using S2VX.Game.Play;
using S2VX.Game.SongSelection;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class EndGameScreenTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Cached]
        private ScreenStack ScreenStack { get; } = new();
        private SongSelectionScreen SongSelectionScreen { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() {
            ScreenStack.Push(SongSelectionScreen);
            Add(ScreenStack);
        }

        [SetUpSteps]
        private void SetUpSteps() => AddStep("Reset to song selection screen", () => SongSelectionScreen.MakeCurrent());

        private static string GetScoreInGridContainerContent(GridContainerContent content) {
            var score = content[0][1] as SpriteText;
            return score.Text.ToString();
        }

        [Test]
        public void Push_DefaultEndGameScreen_ShowsZeroScore() {
            EndGameScreen endGameScreen = null;
            AddStep("Add end game screen", () => SongSelectionScreen.Push(endGameScreen = new(new(), "")));

            // This looks pretty messed up and it kind of is. I am encountering
            // some wacky behavior where a newly pushed screen is asynchronously
            // loaded, causing some weird issues. If I don't have this UntilStep
            // to ensure that the new screen is loaded, the screen could
            // SOMETIMES be loaded by the time future steps run, and that'll
            // lead to irregular errors.
            //
            // To add onto the strangeness, it seems like this behavior only
            // occurs when running through Test Explorer or the pipeline. I had
            // no issues in the visual GUI.
            AddUntilStep("Load end game screen", () => endGameScreen.IsLoaded);

            AddAssert("Shows zero score", () => GetScoreInGridContainerContent(endGameScreen.ScoreStatisticsDisplay.Content) == "0");
        }

        [Test]
        public void Push_Score_ShowsCorrectScore() {
            EndGameScreen endGameScreen = null;
            AddStep("Add end game screen", () => SongSelectionScreen.Push(endGameScreen = new(new() { Score = 1234 }, "")));
            AddUntilStep("Load end game screen", () => endGameScreen.IsLoaded);
            AddAssert("Shows correct score", () => GetScoreInGridContainerContent(endGameScreen.ScoreStatisticsDisplay.Content) == "1234");
        }

        private static string GetTextInTextFlowContainer(TextFlowContainer container) {
            var spriteText = container[0] as SpriteText;
            return spriteText.Text.ToString();
        }

        [Test]
        public void Push_StoryDirectory_ShowsCorrectDirectory() {
            EndGameScreen endGameScreen = null;
            AddStep("Add end game screen", () => SongSelectionScreen.Push(endGameScreen = new(new(), "StoryDirectory")));
            AddUntilStep("Load end game screen", () => endGameScreen.IsLoaded);
            AddAssert("Shows correct directory", () => GetTextInTextFlowContainer(endGameScreen.Border.TxtPath) == "StoryDirectory");
        }

        private PlayScreen CreatePlayScreen() =>
            new(
                false,
                new("HeadlessTests/SongPreviewTests/ValidStory.s2ry", false),
                S2VXTrack.Open(Path.Combine("TestTracks", "10-seconds-of-silence.mp3"), Audio)
            );

        [Test]
        public void OnTrackCompleted_PlayScreenTrackComplete_PushesEndGameScreen() {
            PlayScreen playScreen = null;
            AddStep("Add play screen", () => SongSelectionScreen.Push(playScreen = CreatePlayScreen()));
            AddUntilStep("Load play screen", () => playScreen.IsLoaded);
            AddStep("Complete track", () => playScreen.OnTrackCompleted());
            AddAssert("Pushes end game screen", () => ScreenStack.CurrentScreen is EndGameScreen);
        }

        [Test]
        public void OnTrackCompleted_EditorTestPlayTrackComplete_StaysOnPlayScreen() {
            PlayScreen playScreen = null;
            AddStep("Add play screen", () => SongSelectionScreen.Push(playScreen = new(
                true,
                new("HeadlessTests/SongPreviewTests/ValidStory.s2ry", false),
                S2VXTrack.Open(Path.Combine("TestTracks", "10-seconds-of-silence.mp3"), Audio)
            )));
            AddUntilStep("Load play screen", () => playScreen.IsLoaded);
            AddStep("Complete track", () => playScreen.OnTrackCompleted());
            AddAssert("Stays on play screen", () => ScreenStack.CurrentScreen is PlayScreen);
        }

        [Test]
        public void OnExit_SongSelectionToPlayToEndGame_GoesBackToSongSelection() {
            PlayScreen playScreen = null;
            AddStep("Add play screen", () => SongSelectionScreen.Push(playScreen = CreatePlayScreen()));
            AddUntilStep("Load play screen", () => playScreen.IsLoaded);
            EndGameScreen endGameScreen = null;
            AddStep("Add end game screen", () => playScreen.Push(endGameScreen = new(new(), "")));
            AddUntilStep("Load end game screen", () => endGameScreen.IsLoaded);
            AddStep("Click outer border", () => endGameScreen.Border.BorderOuter.Click());
            AddAssert("Goes back to song selection", () => ScreenStack.CurrentScreen is SongSelectionScreen);
        }
    }
}

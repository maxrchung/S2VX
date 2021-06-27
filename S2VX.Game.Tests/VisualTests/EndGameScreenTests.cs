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
        private SongSelectionScreen SongSelectionScreen { get; } = new SongSelectionScreen();

        [BackgroundDependencyLoader]
        private void Load() {
            ScreenStack.Push(SongSelectionScreen);
            Add(ScreenStack);
        }


        [SetUpSteps]
        private void SetUpSteps() => SongSelectionScreen.MakeCurrent();

        private static string GetScoreInGridContainerContent(GridContainerContent content) {
            var score = content[0][1] as SpriteText;
            return score.Text.ToString();
        }

        [Test]
        public void Push_DefaultEndGameScreen_ShowsZeroScore() {
            EndGameScreen endGameScreen = null;
            AddStep("Set end game screen", () => endGameScreen = new EndGameScreen(new() { Score = 1234 }, ""));
            AddStep("Add end game screen", () => ScreenStack.Push(endGameScreen));
            AddAssert("Shows zero score", () => GetScoreInGridContainerContent(endGameScreen.ScoreStatisticsDisplay.Content) == "0");
        }

        [Test]
        public void Push_Score_ShowsCorrectScore() {
            EndGameScreen endGameScreen = null;
            AddStep("Set end game screen", () => endGameScreen = new EndGameScreen(new() { Score = 1234 }, ""));
            AddStep("Add end game screen", () => ScreenStack.Push(endGameScreen));
            AddAssert("Shows correct score", () => GetScoreInGridContainerContent(endGameScreen.ScoreStatisticsDisplay.Content) == "1234");
        }

        private static string GetTextInTextFlowContainer(TextFlowContainer container) {
            var spriteText = container[0] as SpriteText;
            return spriteText.Text.ToString();
        }

        [Test]
        public void Push_StoryDirectory_ShowsCorrectDirectory() {
            EndGameScreen endGameScreen = null;
            AddStep("Set end game screen", () => endGameScreen = new EndGameScreen(new(), "StoryDirectory"));
            AddStep("Add end game screen", () => ScreenStack.Push(endGameScreen));
            AddAssert("Shows correct directory", () => GetTextInTextFlowContainer(endGameScreen.Border.TxtPath) == "1234");
        }

        [Test]
        public void OnTrackCompleted_PlayScreenTrackComplete_PushesEndGameScreen() {
            PlayScreen playScreen = null;
            AddStep("Set play screen", () => playScreen = new PlayScreen(
                false,
                new(),
                S2VXTrack.Open(Path.Combine("TestTracks", "10-seconds-of-silence.mp3"), Audio)
            ));
            AddStep("Add play screen", () => ScreenStack.Push(playScreen));
            AddStep("Complete track", () => playScreen.OnTrackCompleted());
            AddAssert("Pushes end game screen", () => ScreenStack.CurrentScreen is EndGameScreen);
        }

        [Test]
        public void OnTrackCompleted_EditorTestPlayTrackComplete_DoesNotPushEndGameScreen() {
            PlayScreen playScreen = null;
            AddStep("Set play screen", () => playScreen = new PlayScreen(
                true,
                new(),
                S2VXTrack.Open(Path.Combine("TestTracks", "10-seconds-of-silence.mp3"), Audio)
            ));
            AddStep("Add play screen", () => ScreenStack.Push(playScreen));
            AddStep("Complete track", () => playScreen.OnTrackCompleted());
            AddAssert("Does not push end game screen", () => ScreenStack.CurrentScreen is not EndGameScreen);
        }

        [Test]
        public void Exit_SongSelectionToPlayToEndGame_GoesBackToSongSelection() {
            PlayScreen playScreen = null;
            AddStep("Set play screen", () => playScreen = new PlayScreen(
                true,
                new(),
                S2VXTrack.Open(Path.Combine("TestTracks", "10-seconds-of-silence.mp3"), Audio)
            ));
            AddStep("Add play screen", () => ScreenStack.Push(playScreen));
            EndGameScreen endGameScreen = null;
            AddStep("Set end game screen", () => endGameScreen = new EndGameScreen(new() { Score = 1234 }, ""));
            AddStep("Add end game screen", () => ScreenStack.Push(endGameScreen));
            AddStep("Click outer border", () => endGameScreen.Border.BorderOuter.Click());
            AddAssert("Goes back to song selection", () => ScreenStack.CurrentScreen is not EndGameScreen);
        }
    }
}

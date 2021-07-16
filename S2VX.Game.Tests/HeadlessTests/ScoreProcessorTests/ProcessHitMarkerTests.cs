//using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
//using osuTK;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHitMarkerTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; } = new();

        private Notes Notes { get; set; }
        private HitMarkers HitMarkers { get; set; }
        private ScoreProcessor ScoreProcessor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(ScoreProcessor);

        //[SetUpSteps]
        //public void SetUpSteps() {
        //    Notes = Story.Notes;
        //    HitMarkers = Story.HitMarkers;
        //    AddStep("Clear any remaining hit markers", () => HitMarkers.Reset());
        //    AddStep("Reset score processor", () => ScoreProcessor.Reset());
        //}

        //private void ProcessHit(double scoreTime) =>
        //    AddStep("Process hit", () => ScoreProcessor.ProcessHit(scoreTime, 0, new Vector2(100, 100)));

        //[Test]
        //public void ProcessHit_PerfectHit_CreatesPerfectMarker() {
        //    ProcessHit(0);
        //    AddAssert("Creates a perfect hit marker", () => HitMarkers.Children.First.Colour == Notes.PerfectColor);
        //}

        //[Test]
        //public void ProcessHit_EarlyHit_CreatesEarlyMarker() {
        //    ProcessHit(-Notes.PerfectThreshold - 1);
        //    AddAssert("Creates an early hit marker", () => HitMarkers.Children.First.Colour == Notes.EarlyColor);
        //}

        //[Test]
        //public void ProcessHit_LateHit_CreatesLateMarker() {
        //    ProcessHit(Notes.PerfectThreshold + 1);
        //    AddAssert("Creates a late hit marker", () => HitMarkers.Children.First.Colour == Notes.LateColor);
        //}

        //[Test]
        //public void ProcessHit_EarlyMissHit_CreatesMissMarker() {
        //    ProcessHit(-Notes.HitThreshold - 1);
        //    AddAssert("Creates a miss hit marker", () => HitMarkers.Children.First.Colour == Notes.MissColor);
        //}

        //[Test]
        //public void ProcessHit_LateMissHit_CreatesMissMarker() {
        //    ProcessHit(Notes.HitThreshold + 1);
        //    AddAssert("Creates a miss hit marker", () => HitMarkers.Children.First.Colour == Notes.MissColor);
        //}

        //[Test]
        //public void ProcessHit_BeforeMissHit_NoHitMarkers() {
        //    ProcessHit(-Notes.MissThreshold - 1);
        //    AddAssert("There are no hit markers", () => HitMarkers.Children.Count == 0);
        //}

        //[Test]
        //public void ProcessHit_AfterMissHit_CreatesMissMarker() {
        //    ProcessHit(Notes.MissThreshold + 1);
        //    AddAssert("Creates a miss hit marker", () => HitMarkers.Children.First.Colour == Notes.MissColor);
        //}
    }
}

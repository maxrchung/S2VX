//using NUnit.Framework;
//using osu.Framework.Allocation;
//using osu.Framework.Audio;
//using osu.Framework.Screens;
//using osu.Framework.Testing;
//using osu.Framework.Utils;
//using S2VX.Game.Editor;
//using S2VX.Game.Story;
//using S2VX.Game.Story.Command;
//using S2VX.Game.Story.Note;
//using System.IO;

//namespace S2VX.Game.Tests.VisualTests {
//    public class EditorApproachRateDisplayTests : S2VXTestScene {
//        private EditorScreen Editor { get; set; }

//        [BackgroundDependencyLoader]
//        private void Load(AudioManager audio) {
//            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
//            var track = S2VXTrack.Open(audioPath, audio);

//            Add(new ScreenStack(Editor = new EditorScreen(new S2VXStory(), track)));
//        }

//        //SetUp Steps reset the UI scroll bar approach rate

//        //public void movemouse_overthecontrol and scroll up and down modify and check actual value() scroll down

//        //^ that but scroll up

//        //scroll on left end so it doesnt scroll

//        // scroll on right end so it doesnt scroll
//    }
//}

using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {
        protected override bool OnClick(ClickEvent e) {
            Console.WriteLine("GameNote clicked");
            return false;
        }
    }
}

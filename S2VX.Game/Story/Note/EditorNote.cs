using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class EditorNote : S2VXNote {
        protected override bool OnClick(ClickEvent e) {
            Console.WriteLine("You clicked a EditorNote");
            return false;
        }
    }
}

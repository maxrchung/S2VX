using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {

        [Resolved]
        private S2VXScore Score { get; set; }

        protected override bool OnClick(ClickEvent e) {
            Score.Value += Math.Abs((int)(EndTime - Clock.CurrentTime));
            Console.WriteLine($"{EndTime} clicked at {Clock.CurrentTime}, score is {Score.Value}");
            return false;
        }
    }
}

using S2VX.Game.Editor.Containers;

namespace S2VX.Game.Editor.UserInterface {
    public class TapPanel : S2VXOverlayContainer {
        private double PreviousTime { get; set; }
        private double NumberOfTaps { get; set; }
        private double TotalTapTime { get; set; }
    }
}

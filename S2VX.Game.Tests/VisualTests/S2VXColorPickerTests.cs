
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using S2VX.Game.Editor.ColorPicker;

namespace S2VX.Game.Tests.VisualTests {
    public class S2VXColorPickerTests : TestScene {
        public S2VXColorPickerTests() {
            Add(new S2VXColorPicker {
                Margin = new MarginPadding(20),
            });
        }
    }
}

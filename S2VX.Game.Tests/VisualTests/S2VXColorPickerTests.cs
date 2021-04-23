using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using S2VX.Game.Editor.ColorPicker;

namespace S2VX.Game.Tests.VisualTests {
    public class S2VXColorPickerTests : TestScene {
        public S2VXColorPickerTests() {
            S2VXColorPicker colorPicker;
            SpriteText colorText;
            Add(colorPicker = new S2VXColorPicker { Margin = new MarginPadding(20) });
            Add(colorText = new SpriteText { });
            colorPicker.Current.BindValueChanged(value => colorText.Text = value.NewValue.ToString());
        }
    }
}

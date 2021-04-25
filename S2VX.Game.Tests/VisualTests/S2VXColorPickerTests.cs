using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using S2VX.Game.Editor.ColorPicker;

namespace S2VX.Game.Tests.VisualTests {
    public class S2VXColorPickerTests : TestScene {
        public S2VXColorPickerTests() {
            var colorPicker = new S2VXColorPicker() { Margin = new(20) };
            var colorText = new SpriteText();
            colorPicker.Current.BindValueChanged(value => colorText.Text = value.NewValue.ToString());

            Add(colorPicker);
            Add(colorText);
        }
    }
}

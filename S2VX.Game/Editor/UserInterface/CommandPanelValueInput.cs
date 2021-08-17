using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using S2VX.Game.Editor.ColorPicker;
using S2VX.Game.Editor.Containers;
using System;
using System.Globalization;

namespace S2VX.Game.Editor.UserInterface {
    public class CommandPanelValueInput : Container {

        public TextBox TxtValue { get; } = CreateErrorTextBox();

        private static BasicTextBox CreateErrorTextBox() =>
            new() {
                Size = CommandPanel.InputSize,
                BorderColour = Color4.Red,
                Masking = true
            };

        public IconButton BtnApplyCurrentTime { get; } = new IconButton() {
            Icon = FontAwesome.Solid.Clock,
            Alpha = 0,
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Position = new(-5, 5),
            Size = new(CommandPanel.InputSize.Y - 10)
        };

        public BasicButton BtnToggle { get; } = new BasicButton() {
            Alpha = 0,
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Position = new(-5, 5),
            Size = new(CommandPanel.InputSize.Y - 10)
        };

        public S2VXColorPicker ColorPicker { get; } = new S2VXColorPicker() {
            Alpha = 0,
            Position = new(0, CommandPanel.InputSize.Y)
        };

        public void UseColorPicker(bool isColorValue) {
            ColorPicker.Hide();
            if (isColorValue) {
                BtnToggle.Show();
            } else {
                BtnToggle.Hide();
            }
        }

        private void ApplyCurrentTime(Func<double> currentTimeDelegate) =>
            TxtValue.Current.Value = currentTimeDelegate().ToString(CultureInfo.InvariantCulture);

        private void TogglePicker() {
            if (ColorPicker.Alpha == 0) {
                ColorPicker.Show();
            } else {
                ColorPicker.Hide();
            }
        }

        private void BindTxtValueChange(ValueChangedEvent<string> value) {
            if (value.NewValue == null) {
                return;
            }

            try {
                var newColor = S2VXUtils.StringToColor4(value.NewValue);
                ColorPicker.Current.Value = newColor;
            } catch {
                // Ignore any parsing errors
            }
        }

        private void BindColorPickerChange(ValueChangedEvent<Color4> colorValue) {
            var newColor = colorValue.NewValue;
            BtnToggle.BackgroundColour = new(newColor.R, newColor.G, newColor.B, 1);
            TxtValue.Current.Value = $"({newColor.R},{newColor.G},{newColor.B})";
        }

        // Bindings need to be set up early so that they can trigger before
        // Load() happens
        public CommandPanelValueInput(Func<double> currentTimeDelegate = null) {
            BtnApplyCurrentTime.Action = () => ApplyCurrentTime(currentTimeDelegate);
            if (currentTimeDelegate != null) {
                BtnApplyCurrentTime.Show();
            }
            BtnToggle.Action = TogglePicker;
            TxtValue.Current.BindValueChanged(BindTxtValueChange);
            ColorPicker.Current.BindValueChanged(BindColorPickerChange);
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Size = CommandPanel.InputSize;

            Children = new Drawable[] {
                TxtValue,
                BtnApplyCurrentTime,
                BtnToggle,
                ColorPicker
            };
        }
    }
}

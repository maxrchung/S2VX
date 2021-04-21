// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.

using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Editor.ColorPicker {
    public class S2VXColorPicker : Container, IHasCurrentValue<Color4> {
        // Change current value will cause recursive change, so need a record to disable this change.
        private readonly Cached InternalUpdate = new Cached();

        private readonly BindableWithCurrent<Color4> CurrentColor =
            new BindableWithCurrent<Color4> { Default = Color4.White };

        public Bindable<Color4> Current {
            get => CurrentColor.Current;
            set => CurrentColor.Current = value;
        }

        public Color4 BackgroundColour {
            get => Background.Colour;
            set => Background.FadeColour(value);
        }

        protected Box Background { get; }
        protected PickerAreaContainer PickerArea { get; }
        protected HueSlideContainer HueSlider { get; }
        protected TextBox ColorCodeTextBox { get; }
        protected Box PreviewColorBox { get; }

        public S2VXColorPicker() {
            AutoSizeAxes = Axes.Both;
            Children = new Drawable[] {
                Background = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray
                },
                new FillFlowContainer {
                    Margin = new MarginPadding(10),
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[] {
                        PickerArea = new PickerAreaContainer {
                            Size = new Vector2(200),
                        },
                        HueSlider = new HueSlideContainer {
                            RelativeSizeAxes = Axes.X,
                            Height = 50,
                        },
                        new Container {
                            RelativeSizeAxes = Axes.X,
                            Height = 40,
                            Padding = new MarginPadding(10),
                            Child = new GridContainer {
                                RelativeSizeAxes = Axes.Both,
                                Content = new[] {
                                    new Drawable[] {
                                        ColorCodeTextBox = new HexTextBox {
                                            RelativeSizeAxes = Axes.Both,
                                            LengthLimit = 7
                                        },
                                        PreviewColorBox = new Box {
                                            RelativeSizeAxes = Axes.Both
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            HueSlider.Hue.BindTo(PickerArea.Hue);

            Current.BindValueChanged(value => {
                var newColor = value.NewValue;

                // Update text and preview area
                ColorCodeTextBox.Text = newColor.ToHex().ToUpperInvariant();
                PreviewColorBox.Colour = newColor;

                // Prevent internal update cause recursive
                if (!InternalUpdate.IsValid) {
                    return;
                }

                // Assign canvas and scroller to change to current color
                (var h, var s, var v) = newColor.ToHSV();
                HueSlider.Hue.Value = h;
                PickerArea.Saturation.Value = s;
                PickerArea.Value.Value = v;
            }, true);

            // If text changed is valid, change current color.
            ColorCodeTextBox.Current.BindValueChanged(value => {
                if (value.NewValue.Replace("#", "", System.StringComparison.Ordinal).Length != 6) {
                    return;
                }

                Current.Value = Color4Extensions.FromHex(value.NewValue);
            });

            // Update scroll result
            PickerArea.Hue.BindValueChanged(_ => InternalUpdate.Invalidate());
            PickerArea.Saturation.BindValueChanged(_ => InternalUpdate.Invalidate());
            PickerArea.Value.BindValueChanged(_ => InternalUpdate.Invalidate());
        }

        protected override void Update() {
            base.Update();

            if (!InternalUpdate.IsValid) {
                UpdateHSL();
            }
        }

        private void UpdateHSL() {
            var h = PickerArea.Hue.Value;
            var s = PickerArea.Saturation.Value;
            var v = PickerArea.Value.Value;

            // Update current color
            var color = Color4Extensions.FromHSV(h, s, v);
            Current.Value = color;

            // Set to valid
            InternalUpdate.Validate();
        }
    }
}

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.

using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using System;

namespace S2VX.Game.Editor.ColorPicker {
    public class PickerAreaContainer : Container {
        public BindableFloat Hue { get; } = new();
        public BindableFloat Saturation { get; } = new();
        public BindableFloat Value { get; } = new();

        protected static Drawable CreatePicker() => new Circle {
            Size = new(10),
            Colour = Color4.Red,
            Origin = Anchor.Centre
        };

        public PickerAreaContainer() {
            Box horizontalBackground;
            Box verticalBackground;
            Drawable picker;
            Children = new[] {
                new Box {
                    RelativeSizeAxes = Axes.Both,
                },
                horizontalBackground = new Box {
                    RelativeSizeAxes = Axes.Both,
                },
                verticalBackground = new Box {
                    RelativeSizeAxes = Axes.Both,
                },
                picker = CreatePicker()
            };

            // Re-calculate display color if HSV's hue changed.
            Hue.BindValueChanged(value => {
                var color = Color4Extensions.FromHSV(value.NewValue, 1, 1);
                horizontalBackground.Colour = ColourInfo.GradientHorizontal(new(), color);
                verticalBackground.Colour = ColourInfo.GradientVertical(new(), Color4.Black);
            }, true);

            // Update picker position
            Saturation.BindValueChanged(value => picker.X = value.NewValue * DrawWidth);
            Value.BindValueChanged(value => picker.Y = (1 - value.NewValue) * DrawHeight);
        }

        protected override bool OnClick(ClickEvent e) {
            HandleMouseInput(e);
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) {
            HandleMouseInput(e);
            return true;
        }

        protected override void OnDrag(DragEvent e) => HandleMouseInput(e);

        protected override void OnDragEnd(DragEndEvent e) => HandleMouseInput(e);

        private void HandleMouseInput(UIEvent e) {
            var position = ToLocalSpace(e.ScreenSpaceMousePosition);
            Saturation.Value = Math.Clamp(position.X / DrawWidth, 0, 1);
            Value.Value = Math.Clamp(1 - position.Y / DrawHeight, 0, 1);
        }
    }
}

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Editor.ColorPicker {
    public class HueSlideContainer : Container {
        public BindableFloat Hue { get; } = new BindableFloat();

        protected static Drawable CreatePicker() => new Triangle {
            Size = new Vector2(15),
            Colour = Color4.Red,
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.TopCentre
        };

        public HueSlideContainer() {
            Drawable picker;
            Padding = new MarginPadding { Bottom = 20 };
            Children = new[] {
                new GridContainer {
                    RelativeSizeAxes = Axes.Both,
                    Content = new[] {
                        new Drawable[] {
                            new GradientPart(Color4.Red, Color4.Magenta),
                            new GradientPart(Color4.Magenta, Color4.Blue),
                            new GradientPart(Color4.Blue, Color4.Aqua),
                            new GradientPart(Color4.Aqua, Color4.Lime),
                            new GradientPart(Color4.Lime, Color4.Yellow),
                            new GradientPart(Color4.Yellow, Color4.Red),
                        }
                    }
                },
                picker = CreatePicker()
            };

            // Update picker position
            Hue.BindValueChanged(value => picker.X = value.NewValue / 360 * DrawWidth);
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
            var xPosition = ToLocalSpace(e.ScreenSpaceMousePosition).X;
            var percentage = Math.Clamp(xPosition / DrawWidth, 0, 1);
            Hue.Value = percentage * 360;
        }

        private class GradientPart : Box {
            public GradientPart(Color4 start, Color4 end) {
                RelativeSizeAxes = Axes.Both;
                Colour = ColourInfo.GradientHorizontal(start, end);
            }
        }
    }
}

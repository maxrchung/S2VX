﻿using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using osuTK;

namespace S2VX.Game {
    public class IconButton : Button {

        // Can be initialized to use a Texture or FontAwesome Icon
        public string TextureName { get; set; }
        public IconUsage Icon { get; set; }

        public string LabelText { get; set; } = "";
        public int LabelTextSize { get; set; } = 20;

        public Color4 BackgroundColour {
            get => Image.Colour;
            set => Image.FadeColour(value);
        }

        private Color4? FlashColour_;

        /// <summary>
        /// The colour the background will flash with when this button is clicked.
        /// </summary>
        public Color4 FlashColour {
            get => FlashColour_ ?? BackgroundColour;
            set => FlashColour_ = value;
        }

        /// <summary>
        /// The additive colour that is applied to the background when hovered.
        /// </summary>
        public Color4 HoverColour {
            get => Hover.Colour;
            set => Hover.FadeColour(value);
        }

        private Color4 DisabledColour_ = Color4.Gray;

        /// <summary>
        /// The additive colour that is applied to this button when disabled.
        /// </summary>
        public Color4 DisabledColour {
            get => DisabledColour_;
            set {
                if (DisabledColour_ == value) {
                    return;
                }

                DisabledColour_ = value;
                Enabled.TriggerChange();
            }
        }

        /// <summary>
        /// The duration of the transition when hovering.
        /// </summary>
        public double HoverFadeDuration { get; set; } = 200;

        /// <summary>
        /// The duration of the flash when this button is clicked.
        /// </summary>
        public double FlashDuration { get; set; } = 200;

        /// <summary>
        /// The duration of the transition when toggling the Enabled state.
        /// </summary>
        public double DisabledFadeDuration { get; set; } = 200;

        protected Box Hover { get; set; }
        protected Box Background { get; set; }
        protected Drawable Image { get; set; }

        [BackgroundDependencyLoader]
        private void Load(TextureStore store) {
            if (!Icon.Equals(new IconUsage())) {    // Equivalent to != null
                var iconScale = 0.8f;
                Image = new SpriteIcon {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(iconScale),
                    Icon = Icon,
                };
            } else if (TextureName != null) {
                Image = new Sprite {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = FrameworkColour.BlueGreen,
                    Texture = store.Get(TextureName) ?? Texture.WhitePixel, // This will handle the else condition
                };
            }
            AddRange(new Drawable[] {
                Background = new Box {
                    Width = Width,
                    Height = Height,
                    Colour = FrameworkColour.BlueGreen,
                },
                Image,
                Hover = new Box
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White.Opacity(.1f),
                    Blending = BlendingParameters.Additive
                },
                new TextFlowContainer(s => s.Font = new FontUsage("default", LabelTextSize)) {
                    Origin = Anchor.TopLeft,
                    Anchor = Anchor.TopLeft,
                    Text = LabelText,
                },
            });
        }

        protected override bool OnClick(ClickEvent e) {
            if (Enabled.Value) {
                Image.FlashColour(FlashColour, FlashDuration);
            }

            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e) {
            if (Enabled.Value) {
                Hover.FadeIn(HoverFadeDuration);
            }

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            base.OnHoverLost(e);

            Hover.FadeOut(HoverFadeDuration);
        }

        //private void EnabledChanged(ValueChangedEvent<bool> e) =>
        //    this.FadeColour(e.NewValue ? Color4.White : DisabledColour, DisabledFadeDuration, Easing.OutQuint);
    }
}
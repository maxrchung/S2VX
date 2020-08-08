using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using SixLabors.ImageSharp;

namespace S2VX.Game
{
    class NotesTimeline : CompositeDrawable
    {
        [Resolved]
        private Story story { get; set; } = null;
        private List<RelativeBox> ticks { get; set; } = new List<RelativeBox>();
        private Container tickBar { get; set; } = new Container
        {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
            Width = timelineWidth / 1.25f,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            X = -0.075f,
            Y = 0.3f,
        };

        public static readonly int[] validBeatDivisors = { 1, 2, 3, 4, 6, 8, 12, 16 };
        public static readonly Color4[][] tickColoring = new Color4[][]
        {
            new Color4[] { Color4.White},
            new Color4[] { Color4.White, Color4.Red},
            new Color4[] { Color4.White, Color4.Pink, Color4.Pink},
            new Color4[] { Color4.White, Color4.Blue, Color4.Red, Color4.Blue},
            new Color4[] { Color4.White, Color4.Orange, Color4.Pink, Color4.Red, Color4.Pink, Color4.Orange},
            new Color4[] { Color4.White, Color4.Yellow, Color4.Blue, Color4.Yellow, Color4.Red, Color4.Yellow, Color4.Blue, Color4.Yellow},
            new Color4[] { Color4.White, Color4.Brown, Color4.Orange, Color4.Blue, Color4.Pink, Color4.Brown, Color4.Red, Color4.Brown, Color4.Pink, Color4.Blue, Color4.Orange, Color4.Brown},
            new Color4[] { Color4.White, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Red, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown},
        }; // lmao

        private const float timelineHeight = 0.075f;
        private const float timelineWidth = 1.0f;

        private int divisorIndex = 3;
        private int divisor = 4;

        private SpriteText txtBeatSnapDivisorLabel { get; set; } = new SpriteText
        {
            Text = "Beat Snap Divisor",
            Font = new FontUsage("default", 23, "500"),
        };

        private SpriteText txtBeatSnapDivisor { get; set; } = new SpriteText
        {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 23, "500"),
            Text = "1/4",
            X = 0.4f,
            Y = 0.275f,
        };

        private float TextSize
        {
            set
            {
                txtBeatSnapDivisorLabel.Font = txtBeatSnapDivisorLabel.Font.With(size: value);
                txtBeatSnapDivisor.Font = txtBeatSnapDivisorLabel.Font;
            }
        }

        private void changeBeatDivisor(int delta)
        {
            divisorIndex = Math.Clamp(divisorIndex + delta, 0, 7);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Height = timelineHeight;
            Width = timelineWidth;
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                new RelativeBox
                {
                    Colour = Color4.Black.Opacity(0.9f)
                },
                tickBar,
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Height = 0.565f,
                    Width = timelineWidth / 10,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    X = -0.035f,
                    Y = 0.05f,
                    Direction = FillDirection.Vertical,

                    Children = new Drawable[]
                    {
                        txtBeatSnapDivisorLabel,
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,

                            Children = new Drawable[]
                            {
                                new BasicButton
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.Both,
                                    Width = 0.25f,
                                    Action = () => changeBeatDivisor(-1),
                                    Text = "-",
                                },
                                new Box
                                {
                                    Colour = Color4.Black,
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.Both,
                                    Width = 0.5f,
                                    X = 0.25f,
                                },
                                new BasicButton
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.Both,
                                    Width = 0.25f,
                                    X = 0.75f,
                                    Action = () => changeBeatDivisor(1),
                                    Text = "+",
                                },
                                txtBeatSnapDivisor,
                            }
                        }
                    }
                }
            };
        }

        protected override void Update()
        {
            tickBar.Clear();
            tickBar.Add(new RelativeBox
            {
                Colour = Color4.White,
                Height = timelineHeight / 3.5f,
            });

            tickBar.Add(new RelativeBox
            {
                Colour = Color4.White,
                Width = timelineWidth / 350,
                Height = 0.8f,
                Anchor = Anchor.TopCentre,
                Y = 0.1f,
            });

            var offset = 0; // temp
            var BPM = 242; // temp
            var sectionLength = 2; // temp until tickBar is zoomable
            var totalSeconds = story.Track.Length / 1000;
            var BPS = BPM / 60f;
            var numTicks = BPS * totalSeconds;
            var tickSpacing = (1 / numTicks) * (totalSeconds / sectionLength);
            var timeBetweenTicks = story.Track.Length / numTicks;
            var midTickOffset = (story.GameTime - offset) % timeBetweenTicks;
            var relativeMidTickOffset = midTickOffset / (sectionLength * 1000);

            divisor = validBeatDivisors[divisorIndex];
            var microTickSpacing = tickSpacing / divisor;

            for (var tickPos = ((0.5f - relativeMidTickOffset) % tickSpacing) - tickSpacing; tickPos <= 1;)
            {
                var bigTick = true;
                for (var beat = 0; beat < divisor && tickPos <= 1; ++beat)
                {
                    if (tickPos >= 0)
                    {
                        var height = 0.15f;
                        var y = 0.425f;
                        var width = timelineWidth / 410;

                        if (bigTick)
                        {
                            height = 0.3f;
                            y = 0.35f;
                            width = timelineWidth / 350;
                        }

                        tickBar.Add(new RelativeBox
                        {
                            Colour = tickColoring[divisorIndex][beat],
                            Width = width,
                            Height = height,
                            X = (float)tickPos,
                            Y = y,
                            Anchor = Anchor.TopLeft,
                            Depth = 1,
                        });
                    }
                    tickPos += microTickSpacing;
                    bigTick = false;
                }
            }

            TextSize = story.DrawWidth / 60;
            txtBeatSnapDivisor.Text = $"1/{divisor}";
        }
    }
}

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor {
    public class NotesTimeline : CompositeDrawable {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        private Container TickBar { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
            Width = TimelineWidth / 1.25f,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            X = -0.05f,
            Y = 0.3f,
        };

        private static readonly int[] ValidBeatDivisors = { 1, 2, 3, 4, 6, 8, 12, 16 };
        private static readonly Color4[][] TickColoring = new Color4[][]
        {
            new Color4[] { Color4.White },
            new Color4[] { Color4.White, Color4.Red },
            new Color4[] { Color4.White, Color4.Pink, Color4.Pink },
            new Color4[] { Color4.White, Color4.Blue, Color4.Red, Color4.Blue },
            new Color4[] { Color4.White, Color4.Orange, Color4.Pink, Color4.Red, Color4.Pink, Color4.Orange },
            new Color4[] { Color4.White, Color4.Yellow, Color4.Blue, Color4.Yellow, Color4.Red, Color4.Yellow, Color4.Blue, Color4.Yellow },
            new Color4[] { Color4.White, Color4.Brown, Color4.Orange, Color4.Blue, Color4.Pink, Color4.Brown, Color4.Red, Color4.Brown, Color4.Pink, Color4.Blue, Color4.Orange, Color4.Brown },
            new Color4[] { Color4.White, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Red, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown },
        };

        private const float TimelineHeight = 0.075f;
        private const float TimelineWidth = 1.0f;

        private float SectionLength { get; set; } = 2;

        private int DivisorIndex { get; set; } = 3;
        private int Divisor { get; set; } = 4;

        private SpriteText TxtBeatSnapDivisorLabel { get; } = new SpriteText {
            Text = "Beat Snap Divisor",
            Font = new FontUsage("default", 23, "500"),
        };

        private SpriteText TxtBeatSnapDivisor { get; } = new SpriteText {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 23, "500"),
            Text = "1/4",
            X = 0.4f,
            Y = 0.275f,
        };

        private float TextSize {
            set {
                TxtBeatSnapDivisorLabel.Font = TxtBeatSnapDivisorLabel.Font.With(size: value);
                TxtBeatSnapDivisor.Font = TxtBeatSnapDivisorLabel.Font;
            }
        }

        private void ChangeBeatDivisor(int delta) => DivisorIndex = Math.Clamp(DivisorIndex + delta, 0, 7);

        private void HandleZoom(float delta) => SectionLength = Math.Clamp(SectionLength + delta, 0.5f, 10f);

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Height = TimelineHeight;
            Width = TimelineWidth;
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                new RelativeBox
                {
                    Colour = Color4.Black.Opacity(0.9f)
                },
                TickBar,
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Height = 0.565f,
                    Width = TimelineWidth / 10,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    X = -0.025f,
                    Y = 0.05f,
                    Direction = FillDirection.Vertical,

                    Children = new Drawable[]
                    {
                        TxtBeatSnapDivisorLabel,
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
                                    Action = () => ChangeBeatDivisor(-1),
                                    Text = "-",
                                },
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.Both,
                                    Colour = Color4.Black,
                                    Width = 0.5f,
                                    X = 0.25f,
                                },
                                new BasicButton
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.Both,
                                    Width = 0.25f,
                                    X = 0.75f,
                                    Action = () => ChangeBeatDivisor(1),
                                    Text = "+",
                                },
                                TxtBeatSnapDivisor,
                            }
                        }
                    }
                },
                new Container
                {
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Height = 0.75f,
                    Width = 0.03f,
                    X = 0.01f,
                    Y = -0.045f,

                    Children = new Drawable[]
                    {
                        new BasicButton
                        {
                            Colour = Color4.Black,
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Height = 0.5f,
                            Action = () => HandleZoom(-.5f),
                        },
                        new BasicButton
                        {
                            Colour = Color4.Black,
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Height = 0.5f,
                            Y = 0.5f,
                            Action = () => HandleZoom(.5f),
                        },
                        new SpriteIcon
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Icon = FontAwesome.Solid.SearchPlus,
                            Size = new Vector2(.5f),
                        },
                        new SpriteIcon
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Icon = FontAwesome.Solid.SearchMinus,
                            Size = new Vector2(.5f),
                            Y = 0.5f,
                        },
                    }
                }

            };
        }

        protected override void Update() {
            TickBar.Clear();
            TickBar.Add(new RelativeBox {
                Colour = Color4.White,
                Height = TimelineHeight / 3.5f,
            });

            TickBar.Add(new RelativeBox {
                Colour = Color4.White,
                Width = TimelineWidth / 350,
                Height = 0.8f,
                Anchor = Anchor.TopCentre,
                Y = 0.1f,
            });

            var totalSeconds = Story.Track.Length / 1000;
            var bps = Story.BPM / 60f;
            var numTicks = bps * totalSeconds;
            var tickSpacing = 1 / numTicks * (totalSeconds / SectionLength);
            var timeBetweenTicks = Story.Track.Length / numTicks;
            var midTickOffset = (Story.GameTime - Story.Offset) % timeBetweenTicks;
            var relativeMidTickOffset = midTickOffset / (SectionLength * 1000);

            Divisor = ValidBeatDivisors[DivisorIndex];
            var microTickSpacing = tickSpacing / Divisor;

            for (var tickPos = (0.5f - relativeMidTickOffset) % tickSpacing - tickSpacing; tickPos <= 1;) {
                var bigTick = true;
                for (var beat = 0; beat < Divisor && tickPos <= 1; ++beat) {
                    if (tickPos >= 0) {
                        var height = 0.15f;
                        var y = 0.425f;
                        var width = TimelineWidth / 410;

                        if (bigTick) {
                            height = 0.3f;
                            y = 0.35f;
                            width = TimelineWidth / 350;
                        }

                        TickBar.Add(new RelativeBox {
                            Colour = TickColoring[DivisorIndex][beat],
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

            TextSize = Story.DrawWidth / 60;
            TxtBeatSnapDivisor.Text = $"1/{Divisor}";
        }
    }
}

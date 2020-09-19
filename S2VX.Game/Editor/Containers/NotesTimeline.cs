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
using System.Collections.Generic;

namespace S2VX.Game.Editor.Containers {
    public class NotesTimeline : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public Dictionary<S2VXNote, RelativeBox> NoteToTimelineNote { get; } = new Dictionary<S2VXNote, RelativeBox>();

        private Container TickBarContent { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };
        public Container TickBarNoteSelections { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
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
#pragma warning disable MEN002 // Line is too long
            new Color4[] { Color4.White, Color4.Brown, Color4.Orange, Color4.Blue, Color4.Pink, Color4.Brown, Color4.Red, Color4.Brown, Color4.Pink, Color4.Blue, Color4.Orange, Color4.Brown },
            new Color4[] { Color4.White, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Red, Color4.Brown, Color4.Yellow, Color4.Brown, Color4.Blue, Color4.Brown, Color4.Yellow, Color4.Brown },
#pragma warning restore MEN002 // Line is too long
        };

        public const float TimelineHeight = 0.075f;
        public const float TimelineWidth = 1.0f;
        public const float TimelineNoteHeight = 0.6f;
        public const float TimelineNoteWidth = TimelineWidth / 17.5f;

        public float SectionLength { get; private set; } = 2;
        public const int SecondsToMS = 1000;

        public int DivisorIndex { get; set; } = 3;
        public int Divisor { get; private set; } = 4;

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

        public void ChangeBeatDivisor(bool increase) => DivisorIndex = Math.Clamp(DivisorIndex + (increase ? 1 : -1), 0, 7);

        public void HandleZoom(bool zoomIn) => SectionLength = Math.Clamp(SectionLength + (zoomIn ? -0.5f : 0.5f), 0.5f, 10f);

        [BackgroundDependencyLoader]
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
                new Container {
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Width = TimelineWidth / 1.25f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    X = -0.05f,
                    Y = 0.3f,

                    Children = new Drawable[] {
                        new RelativeBox {
                            Colour = Color4.White,
                            Height = TimelineHeight / 3.5f,
                        },
                        new RelativeBox {
                            Colour = Color4.White,
                            Width = TimelineWidth / 350,
                            Height = 0.8f,
                            Anchor = Anchor.TopCentre,
                            Y = 0.1f,
                        },
                        TickBarNoteSelections,
                        TickBarContent,
                    }
                },
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
                                    Action = () => ChangeBeatDivisor(false),
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
                                    Action = () => ChangeBeatDivisor(true),
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
                            Action = () => HandleZoom(false),
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
                            Action = () => HandleZoom(true),
                        },
                        new SpriteIcon
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Icon = FontAwesome.Solid.SearchMinus,
                            Size = new Vector2(.5f),
                        },
                        new SpriteIcon
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Icon = FontAwesome.Solid.SearchPlus,
                            Size = new Vector2(.5f),
                            Y = 0.5f,
                        },
                    }
                }
            };
        }

        public void SnapToTick(bool snapLeft) {
            var numTicks = Story.BPM / 60f * (Editor.Track.Length / SecondsToMS) * Divisor;
            var timeBetweenTicks = Editor.Track.Length / numTicks;
            var leftOffset = (Time.Current - Story.Offset) % timeBetweenTicks;

            var tolerance = 0.02;
            if (snapLeft) {
                leftOffset = leftOffset <= tolerance ? timeBetweenTicks : leftOffset;
                Editor.Seek(Math.Clamp(Time.Current - leftOffset, 0, Editor.Track.Length));
            } else {
                var rightOffset = timeBetweenTicks - leftOffset;
                rightOffset = rightOffset <= tolerance ? timeBetweenTicks : rightOffset;
                Editor.Seek(Math.Clamp(Time.Current + rightOffset, 0, Editor.Track.Length));
            }
        }

        private void AddVisibleNotes() {
            var lowerBound = Time.Current - SectionLength * SecondsToMS / 2;
            var upperBound = Time.Current + SectionLength * SecondsToMS / 2;
            var color = Story.Notes.Colour;
            foreach (var note in Story.Notes.Children) {
                if (lowerBound <= note.EndTime && note.EndTime <= upperBound) {
                    var relativePosition = (note.EndTime - lowerBound) / (SectionLength * SecondsToMS);
                    var visibleNote = new RelativeBox {
                        Colour = color,
                        Alpha = 0.727f,
                        Width = TimelineNoteWidth,
                        Height = TimelineNoteHeight,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                    };
                    NoteToTimelineNote[note] = visibleNote;
                    TickBarContent.Add(visibleNote);
                }
            }
        }

        protected override void Update() {
            NoteToTimelineNote.Clear();
            TickBarContent.Clear();

            var totalSeconds = Editor.Track.Length / SecondsToMS;
            var bps = Story.BPM / 60f;
            var numBigTicks = bps * totalSeconds;
            var tickSpacing = 1 / numBigTicks * (totalSeconds / SectionLength);
            var timeBetweenTicks = Editor.Track.Length / numBigTicks;
            var midTickOffset = (Time.Current - Story.Offset) % timeBetweenTicks;
            var relativeMidTickOffset = midTickOffset / (SectionLength * SecondsToMS);

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

                        TickBarContent.Add(new RelativeBox {
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

            AddVisibleNotes();
        }
    }
}

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
using S2VX.Game.Story.Note;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Containers {
    public class NotesTimeline : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        // Tick information is calculated by GetTickInfo for a given time
        private struct TickInfo {
            public double CurrentTick;
            public double CurrentTickTime;
            public int NearestTick;
            public bool OnTick;
        }

        public double TimeBetweenTicks { get; private set; }
        public int FirstVisibleTick { get; private set; }
        public int LastVisibleTick { get; private set; }

        public Dictionary<S2VXNote, double> SelectedNoteToTime { get; private set; } = new();
        public Dictionary<S2VXNote, RelativeBox> NoteToTimelineNote { get; } = new();

        public Container TickBarContent { get; } = new() {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };
        public Container NoteSelectionIndicators { get; } = new() {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };

        private static readonly int[] ValidBeatDivisors = { 1, 2, 3, 4, 6, 8, 12, 16 };
        private static readonly Color4[][] TickColoring =
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

        private SpriteText TxtBeatSnapDivisorLabel { get; } = new() {
            Text = "Beat Snap Divisor",
            Font = new FontUsage("default", 23, "500"),
        };

        private SpriteText TxtBeatSnapDivisor { get; } = new() {
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
                CreateTimeline(),
                CreateSnapDivisor(),
                CreateZoomButtons()
            };
        }

        private Container CreateTimeline() =>
            new() {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Width = TimelineWidth * 0.8f,
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
                    NoteSelectionIndicators,
                    TickBarContent,
                }
            };

        private FillFlowContainer CreateSnapDivisor() =>
            new() {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Height = 0.565f,
                Width = TimelineWidth / 10,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                X = -0.025f,
                Y = 0.05f,
                Direction = FillDirection.Vertical,

                Children = new Drawable[] {
                    TxtBeatSnapDivisorLabel,
                    new Container {
                        RelativeSizeAxes = Axes.Both,

                        Children = new Drawable[] {
                            new BasicButton {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Width = 0.25f,
                                Action = () => ChangeBeatDivisor(false),
                                Text = "-",
                            },
                            new Box {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Colour = Color4.Black,
                                Width = 0.5f,
                                X = 0.25f,
                            },
                            new BasicButton {
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
            };

        private Container CreateZoomButtons() =>
            new() {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Height = 0.75f,
                Width = 0.03f,
                X = 0.01f,
                Y = -0.045f,

                Children = new Drawable[] {
                    new BasicButton {
                        Colour = Color4.Black,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Height = 0.5f,
                        Action = () => HandleZoom(false),
                    },
                    new BasicButton {
                        Colour = Color4.Black,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Height = 0.5f,
                        Y = 0.5f,
                        Action = () => HandleZoom(true),
                    },
                    new SpriteIcon {
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Icon = FontAwesome.Solid.SearchMinus,
                        Size = new Vector2(.5f),
                    },
                    new SpriteIcon {
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Icon = FontAwesome.Solid.SearchPlus,
                        Size = new Vector2(.5f),
                        Y = 0.5f,
                    },
                }
            };

        public int GetNearestTick(double time) => GetTickInfo(time).NearestTick;

        private TickInfo GetTickInfo(double time) {
            var currentTick = (time - Story.Offset) / TimeBetweenTicks;
            var currentTickTime = currentTick * TimeBetweenTicks;
            var nearestTick = (int)Math.Round(currentTick);
            var nearestTickTime = nearestTick * TimeBetweenTicks;
            var onTick = Math.Abs(nearestTickTime - currentTickTime) <= EditorScreen.TrackTimeTolerance;
            return new TickInfo {
                CurrentTick = currentTick,
                CurrentTickTime = currentTickTime,
                NearestTick = nearestTick,
                OnTick = onTick
            };
        }

        public void SnapToTick(bool snapLeft) {
            var tickInfo = GetTickInfo(Time.Current);
            var leftTickTime = TimeBetweenTicks;
            var rightTickTime = TimeBetweenTicks;
            if (tickInfo.OnTick) {
                leftTickTime *= tickInfo.NearestTick - 1;
                rightTickTime *= tickInfo.NearestTick + 1;
            } else {
                // Current time is between two ticks
                leftTickTime *= Math.Floor(tickInfo.CurrentTick);
                rightTickTime *= Math.Ceiling(tickInfo.CurrentTick);
            }

            leftTickTime += Story.Offset;
            rightTickTime += Story.Offset;

            if (snapLeft) {
                Editor.Seek(Math.Clamp(leftTickTime, 0, Editor.Track.Length));
            } else {
                Editor.Seek(Math.Clamp(rightTickTime, 0, Editor.Track.Length));
            }
        }

        private void AddVisibleNotes() {
            var sectionDuration = SectionLength * SecondsToMS;
            var lowerBound = Time.Current - sectionDuration / 2;
            var upperBound = Time.Current + sectionDuration / 2;
            foreach (var note in Story.Notes.Children) {
                RelativeBox visibleNote = null;
                var color = note.InnerColor;
                // Clamp so that RelativeBoxes never draw outside of lower and upper bound
                var relativePosition = Math.Clamp((note.HitTime - lowerBound) / sectionDuration, 0, 1);

                if (note is EditorHoldNote holdNote) {
                    if (holdNote.HitTime <= upperBound && holdNote.EndTime >= lowerBound) {
                        var relativeEndPosition = Math.Clamp((holdNote.EndTime - lowerBound) / sectionDuration, 0, 1);
                        var width = (float)(TimelineNoteWidth + relativeEndPosition - relativePosition);
                        visibleNote = new RelativeBox {
                            Colour = color,
                            Alpha = 0.727f,
                            Width = width,
                            Height = TimelineNoteHeight,
                            X = (float)relativePosition - TimelineNoteWidth / 2 + width / 2,
                            Y = 0.2f,
                            Anchor = Anchor.TopLeft
                        };
                    }
                } else {
                    if (lowerBound <= note.HitTime && note.HitTime <= upperBound) {
                        visibleNote = new RelativeBox {
                            Colour = color,
                            Alpha = 0.727f,
                            Width = TimelineNoteWidth,
                            Height = TimelineNoteHeight,
                            X = (float)relativePosition,
                            Y = 0.2f,
                            Anchor = Anchor.TopLeft,
                        };
                    }
                }

                if (visibleNote != null) {
                    NoteToTimelineNote[note] = visibleNote;
                    TickBarContent.Add(visibleNote);
                }
            }
        }

        public void AddNoteTimelineSelection(S2VXNote note) => SelectedNoteToTime[note] = note.HitTime;

        public void ClearNoteTimelineSelection() => SelectedNoteToTime.Clear();

        protected override void Update() {
            NoteToTimelineNote.Clear();
            TickBarContent.Clear();
            NoteSelectionIndicators.Clear();

            var totalSeconds = Editor.Track.Length / SecondsToMS;
            var bps = Story.BPM / 60f;
            var numBigTicks = bps * totalSeconds;
            var tickSpacing = 1 / numBigTicks * (totalSeconds / SectionLength);
            var timeBetweenBigTicks = Editor.Track.Length / numBigTicks;
            var midTickOffset = (Time.Current - Story.Offset) % timeBetweenBigTicks;
            var relativeMidTickOffset = midTickOffset / (SectionLength * SecondsToMS);

            Divisor = ValidBeatDivisors[DivisorIndex];
            var microTickSpacing = tickSpacing / Divisor;

            var numTicks = bps * totalSeconds * Divisor;
            TimeBetweenTicks = Editor.Track.Length / numTicks;
            var numVisibleTicks = 0;

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

                        ++numVisibleTicks;
                    }
                    tickPos += microTickSpacing;
                    bigTick = false;
                }
            }

            var tickInfo = GetTickInfo(Time.Current);
            var nearestTick = tickInfo.NearestTick;
            FirstVisibleTick = nearestTick - numVisibleTicks / 2;
            LastVisibleTick = nearestTick + numVisibleTicks / 2;
            if (numVisibleTicks % 2 == 0) {
                // Move one in from the side we're closer to
                if (tickInfo.CurrentTick > nearestTick) {
                    ++FirstVisibleTick;
                } else {
                    --LastVisibleTick;
                }
            }

            TextSize = Story.DrawWidth / 60;
            TxtBeatSnapDivisor.Text = $"1/{Divisor}";

            var lowerBound = Time.Current - SectionLength * SecondsToMS / 2;
            var upperBound = Time.Current + SectionLength * SecondsToMS / 2;
            foreach (var noteAndTime in SelectedNoteToTime) {
                if (lowerBound <= noteAndTime.Value && noteAndTime.Value <= upperBound) {
                    var relativePosition = (noteAndTime.Value - lowerBound) / (SectionLength * SecondsToMS);
                    var indication = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.727f),
                        Width = TimelineNoteWidth + 0.009727f,
                        Height = TimelineNoteHeight + 0.1f,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                    };
                    NoteSelectionIndicators.Add(indication);
                }
            }

            AddVisibleNotes();
        }
    }
}

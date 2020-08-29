using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story;
using SixLabors.ImageSharp.Processing;
using System;
using System.Globalization;

namespace S2VX.Game.Editor {
    public class Timeline : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private Container Bar { get; set; } = new Container();

        private RelativeBox Slider { get; set; } = new RelativeBox {
            Colour = Color4.White,
            Height = 0.5f,
            Width = TimelineWidth / 150,
            Anchor = Anchor.CentreLeft,
            RelativePositionAxes = Axes.None
        };

        private SpriteText TxtClock { get; set; } = new SpriteText();

        private PlaybackRateDisplay PlaybackRateDisplay { get; set; } = new PlaybackRateDisplay {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Width = 0.15f,
            TextAnchor = Anchor.Centre,
        };

        private bool SwitchToPlaying { get; set; }

        private bool DelayDrag { get; set; }

        public bool DisplayMS { get; set; }

        private void SetTextSize(float value) => TxtClock.Font = TxtClock.Font.With(size: value);

        private void UpdateSlider(Vector2 mousePosition) {
            var mousePosX = ToLocalSpace(mousePosition).X;
            var xPosDelta = (DrawWidth - Bar.DrawWidth) / 2;
            var newX = mousePosX - xPosDelta;
            var xLengthRatio = newX / Bar.DrawWidth;
            var newTime = xLengthRatio * Editor.Track.Length;

            var clampedX = Math.Clamp(newX, 0, Bar.DrawWidth);
            Slider.X = clampedX;

            var clampedTime = Math.Clamp(newTime, 0, Editor.Track.Length);
            Editor.Seek(clampedTime);
        }

        private const float TimelineHeight = 0.075f;
        private const float TimelineWidth = 1.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Height = TimelineHeight;
            Width = TimelineWidth;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            InternalChildren = new Drawable[]
            {
                new RelativeBox
                {
                    Colour = Color4.Black.Opacity(0.9f)
                },
                TxtClock = new SpriteText
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Colour = Color4.White,
                    Text = "00:00:00",
                    X = 0.05f,
                    Y = -0.15f,
                    Font = new FontUsage("default", 30, "500"),
                },
                Bar = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.White,
                    Width = TimelineWidth / 1.5f,
                    Children = new Drawable[]
                    {
                        new RelativeBox
                        {
                            Colour = Color4.White,
                            Height = TimelineHeight / 2.5f,
                        },
                        Slider
                    }
                },
                PlaybackRateDisplay,
            };
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            UpdateSlider(e.ScreenSpaceMousePosition);
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) {
            // Pause if we start a drag
            if (Editor.Track.IsRunning) {
                Editor.Play(false);
                SwitchToPlaying = true;
            } else {
                SwitchToPlaying = false;
            }
            return true;
        }

        protected override void OnDrag(DragEvent e) {
            if (!DelayDrag) {
                UpdateSlider(e.ScreenSpaceMousePosition);
                DelayDrag = true;
            }
        }

        protected override void OnDragEnd(DragEndEvent e) {
            if (SwitchToPlaying) {
                Editor.Play(true);
            }
        }

        protected override void Update() {
            DelayDrag = false;
            var songRatio = Time.Current / Editor.Track.Length;
            var newX = songRatio * Bar.DrawWidth;
            Slider.X = (float)Math.Clamp(newX, 0, Bar.DrawWidth);

            if (DisplayMS) {
                TxtClock.Text = Math.Truncate(Math.Clamp(Time.Current, 0, Editor.Track.Length)).ToString(CultureInfo.InvariantCulture);
            } else {
                var time = TimeSpan.FromMilliseconds(Math.Clamp(Time.Current, 0, Editor.Track.Length));
                TxtClock.Text = time.ToString(@"mm\:ss\:fff", CultureInfo.InvariantCulture);
            }

            SetTextSize(Story.DrawWidth / 40);

            if (Time.Current >= Editor.Track.Length) {
                Editor.Play(false);
            }
        }
    }
}

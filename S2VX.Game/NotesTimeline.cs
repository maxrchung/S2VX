using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFmpeg.AutoGen;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

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

        private const float timelineHeight = 0.075f;
        private const float timelineWidth = 1.0f;

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
                new RelativeBox
                {
                    Colour = Color4.Black,
                    Height = 0.6f,
                    Width = timelineWidth / 12,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    X = -0.05f,
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

            var offset = 727; // temp
            var BPM = 222; // temp
            var sectionLength = 6; // temp until tickBar is zoomable
            var totalSeconds = story.Track.Length / 1000;
            var BPS = BPM / 60f;
            var numBeats = BPS * totalSeconds;
            var tickSpacing = (1 / numBeats) * (totalSeconds / sectionLength);
            var timeBetweenTicks = story.Track.Length / numBeats;
            var midTickOffset = (story.GameTime - offset) % timeBetweenTicks;
            var relativeMidTickOffset = midTickOffset / (sectionLength * 1000);

            for (var tickPos = 0 - relativeMidTickOffset; tickPos <= 1; tickPos += tickSpacing)
            {
                if (tickPos >= 0)
                {
                    tickBar.Add(new RelativeBox
                    {
                        Colour = Color4.White,
                        Width = timelineWidth / 350,
                        Height = 0.3f,
                        X = (float)tickPos,
                        Y = 0.35f,
                        Anchor = Anchor.TopLeft,
                    });
                }
            }
        }
    }
}

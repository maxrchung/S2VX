using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class MainScreen : Screen
    {
        [Cached]
        private Camera camera = new Camera();

        private Box background = new Box();

        private BackColorCommand color = new BackColorCommand
        {
            StartTime = 0,
            EndTime = 20000,
            StartColor = Color4.HotPink,
            EndColor = Color4.DodgerBlue,
            Easing = Easing.OutPow10
        };

        [BackgroundDependencyLoader]
        private void load()
        {
            background = new Box
            {
                Colour = Color4.CornflowerBlue,
                RelativeSizeAxes = Axes.Both
            };
            InternalChildren = new Drawable[]
            {
                camera,
                background,
                new Grid()
            };
        }

        protected override void Update()
        {
            if (Time.Current <= color.EndTime)
            {
                background.Colour = color.Apply(Time.Current);
            }
        }
    }
}

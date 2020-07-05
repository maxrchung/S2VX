using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class MainScreen : Screen
    {
        [Cached]
        private Camera camera = new Camera();

        private Box background = new Box
        {
            Colour = Color4.CornflowerBlue,
            RelativeSizeAxes = Axes.Both
        };

        private Grid grid = new Grid();

        private List<Command> commands = new List<Command>();
        private int nextActive = 0;
        private HashSet<Command> actives = new HashSet<Command>();

        [BackgroundDependencyLoader]
        private void load()
        {
            commands.Add(new CameraMoveCommand
            {
                Camera = camera,
                StartTime = 0,
                EndTime = 60000,
                StartPosition = new Vector2(0, 0),
                EndPosition = new Vector2(5, 5),
                Easing = Easing.OutElastic
            });
            commands.Add(new CameraRotateCommand
            {
                Camera = camera,
                StartTime = 0,
                EndTime = 10000,
                StartRotation = 0.0f,
                EndRotation = 360.0f,
                Easing = Easing.InQuart
            });
            commands.Add(new CameraRotateCommand
            {
                Camera = camera,
                StartTime = 10000,
                EndTime = 20000,
                StartRotation = 0.0f,
                EndRotation = -360.0f,
                Easing = Easing.InQuart
            });
            commands.Add(new CameraScaleCommand
            {
                Camera = camera,
                StartTime = 0,
                EndTime = 60000,
                StartScale = new Vector2(0.6f, 0.6f),
                EndScale = new Vector2(0.01f, 0.01f),
                Easing = Easing.InOutBounce
            });
            commands.Add(new BackgroundColorCommand
            {
                Background = background,
                StartTime = 0,
                EndTime = 20000,
                StartColor = Color4.HotPink,
                EndColor = Color4.DodgerBlue,
                Easing = Easing.OutPow10
            });
            commands.Add(new GridAlphaCommand
            {
                Grid = grid,
                StartTime = 0,
                EndTime = 30000,
                StartAlpha = 1.0f,
                EndAlpha = 0.1f,
                Easing = Easing.InOutQuad
            });
            commands.Add(new GridColorCommand
            {
                Grid = grid,
                StartTime = 0,
                EndTime = 10000,
                StartColor = Color4.Coral,
                EndColor = Color4.Black,
                Easing = Easing.InSine
            });
            commands.Add(new GridThicknessCommand
            {
                Grid = grid,
                StartTime = 0,
                EndTime = 10000,
                StartThickness = 0.05f,
                EndThickness = 0.001f,
                Easing = Easing.None
            });
            commands.Sort();

            InternalChildren = new Drawable[]
            {
                camera,
                background,
                grid
            };
        }

        protected override void Update()
        {
            var time = Time.Current;
            // Add new active commands
            while (nextActive != commands.Count && commands[nextActive].StartTime <= time)
            {
                actives.Add(commands[nextActive++]);
            }

            var newActives = new HashSet<Command>();
            foreach (var active in actives)
            {
                // Run active commands
                active.Apply(time);

                // Remove finished commands by replacing actives
                // This is done last so command end will always trigger(?) - Note from 2 years ago
                if (active.EndTime >= time)
                {
                    newActives.Add(active);
                }
            }
            actives = newActives;
        }
    }
}

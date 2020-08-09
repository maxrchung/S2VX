// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game {
    public class SquareContainer : Container {
        public SquareContainer() {
            RelativeSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Masking = true;
        }

        protected override void Update() => Size = Parent.ChildSize.Y < Parent.ChildSize.X ?
                new Vector2(Parent.ChildSize.Y / Parent.ChildSize.X, 1) :
                new Vector2(1, Parent.ChildSize.X / Parent.ChildSize.Y);
    }
}

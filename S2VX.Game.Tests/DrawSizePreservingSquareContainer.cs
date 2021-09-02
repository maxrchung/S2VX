using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game.Tests {
    /// <summary>
    /// This class basically combines the functionality of SquareContainer and
    /// DrawSizePreservingFillContainer. The reason we need this is because
    /// S2VXGameBase is used in both the normal game and the visual GUI, but we
    /// don't want the visual GUI's UI elements (e.g. the step buttons) to be
    /// constrained to a square. So what we do is we have this special class
    /// that is used in S2VXTestScene. This essentially mimicks what shows up in
    /// the normal game but only for a particular section of the visual GUI.
    /// </summary>
    public class DrawSizePreservingSquareContainer : DrawSizePreservingFillContainer {
        public DrawSizePreservingSquareContainer() {
            TargetDrawSize = new(S2VXGameBase.GameWidth);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Masking = true;
        }

        protected override void Update() {
            base.Update();

            Size = Parent.ChildSize.Y < Parent.ChildSize.X ?
                new Vector2(Parent.ChildSize.Y / Parent.ChildSize.X, 1) :
                new Vector2(1, Parent.ChildSize.X / Parent.ChildSize.Y);
        }
    }
}

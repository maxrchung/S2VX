﻿using NUnit.Framework;
using osu.Framework.Allocation;
using osuTK;

namespace S2VX.Game.Tests.VisualTests.S2VXCursorTests {
    public class CursorDefaultTests : S2VXTestScene {
        [Cached]
        private S2VXCursor Cursor { get; set; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(Cursor);

        [Test]
        public void CreateCursor_Defaults_IsDefaultColor() =>
            AddAssert("Is default color", () => Cursor.ActiveCursor.Colour == S2VXColorConstants.LightYellow);

        [Test]
        public void CreateCursor_Defaults_HasDefaultSize() =>
            AddAssert("Has default size", () => Cursor.ActiveCursor.Size == new Vector2(20));

        [Test]
        public void CreateCursor_Defaults_HasNoRotation() =>
            AddAssert("Has no rotation", () => Cursor.ActiveCursor.Rotation == 0);
    }
}

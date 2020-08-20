﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.S2VX.UI {
    [Cached]
    public class S2VXPlayfield : Playfield {
        [BackgroundDependencyLoader]
        private void Load() => AddRangeInternal(new Drawable[]
            {
                HitObjectContainer,
            });
    }
}

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;
using S2VX.Game.Story;

namespace osu.Game.Rulesets.S2VX.UI {
    [Cached]
    public class S2VXPlayfield : Playfield {
        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() => AddRangeInternal(new Drawable[]
            {
                Story,
                HitObjectContainer,
            });
    }
}

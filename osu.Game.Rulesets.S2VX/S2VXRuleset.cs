// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.S2VX.Beatmaps;
using osu.Game.Rulesets.S2VX.Mods;
using osu.Game.Rulesets.S2VX.UI;
using osu.Game.Rulesets.UI;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.S2VX {
    public class S2VXRuleset : Ruleset {
        public override string Description => "a very s2vxruleset ruleset";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) =>
            new DrawableS2VXRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new S2VXBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) =>
            new S2VXDifficultyCalculator(this, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type) {
            var mods = type switch
            {
                ModType.Automation => new[] { new S2VXModAutoplay() },
                _ => new Mod[] { null }
            };
            return mods;
        }

        public override string ShortName => "s2vxruleset";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, S2VXAction.Button1),
            new KeyBinding(InputKey.X, S2VXAction.Button2),
        };

        public override Drawable CreateIcon() => new Icon(ShortName[0]);

        public class Icon : CompositeDrawable {
            public Icon(char c) => InternalChildren = new Drawable[]
                {
                    new Circle
                    {
                        Size = new Vector2(20),
                        Colour = Color4.White,
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = c.ToString(),
                        Font = OsuFont.Default.With(size: 18)
                    }
                };
        }
    }
}

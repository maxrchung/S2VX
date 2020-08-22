// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.S2VX.Objects;
using osu.Game.Rulesets.S2VX.Objects.Drawables;
using osu.Game.Rulesets.S2VX.Replays;
using osu.Game.Rulesets.UI;
using S2VX.Game.Story;
using System.Collections.Generic;

namespace osu.Game.Rulesets.S2VX.UI {
    [Cached]
    public class DrawableS2VXRuleset : DrawableRuleset<S2VXHitObject> {
        [Cached]
        public S2VXStory Story { get; }
        public DrawableS2VXRuleset(S2VXRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods) {
            // For now we'll just try and load a story.json directly
            //var storyPath = @"../../../../../S2VX.Desktop/story.json";
            Story = new S2VXStory();
        }

        protected override Playfield CreatePlayfield() => new S2VXPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new S2VXFramedReplayInputHandler(replay);

        public override DrawableHitObject<S2VXHitObject> CreateDrawableRepresentation(S2VXHitObject h) => new DrawableS2VXHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new S2VXInputManager(Ruleset?.RulesetInfo);
    }
}

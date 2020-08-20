// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.S2VX.Objects;
using osu.Game.Rulesets.S2VX.Replays;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.S2VX.Mods {
    public class S2VXModAutoplay : ModAutoplay<S2VXHitObject> {
        public override Score CreateReplayScore(IBeatmap beatmap) => new Score {
            ScoreInfo = new ScoreInfo {
                User = new User { Username = "sample" },
            },
            Replay = new S2VXAutoGenerator(beatmap).Generate(),
        };
    }
}

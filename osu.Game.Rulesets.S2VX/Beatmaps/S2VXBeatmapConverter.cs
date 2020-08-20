// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.S2VX.Objects;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.S2VX.Beatmaps {
    public class S2VXBeatmapConverter : BeatmapConverter<S2VXHitObject> {
        public S2VXBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset) {
        }

        // todo: Check for conversion types that should be supported (ie. Beatmap.HitObjects.Any(h => h is IHasXPosition))
        // https://github.com/ppy/osu/tree/master/osu.Game/Rulesets/Objects/Types
        public override bool CanConvert() => true;

        protected override IEnumerable<S2VXHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap) {
            yield return new S2VXHitObject {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Position = (original as IHasPosition)?.Position ?? Vector2.Zero,
            };
        }
    }
}

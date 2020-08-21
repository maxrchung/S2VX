// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.S2VX.Objects;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.S2VX.Beatmaps {
    public class S2VXBeatmapConverter : BeatmapConverter<S2VXHitObject> {
        public S2VXBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset) {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        protected override IEnumerable<S2VXHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap) {
            yield return new S2VXHitObject {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Position = (original as IHasPosition)?.Position ?? Vector2.Zero,
            };
        }
    }
}

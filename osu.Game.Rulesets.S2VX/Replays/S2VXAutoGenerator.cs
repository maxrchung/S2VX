// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.S2VX.Objects;
using System.Collections.Generic;

namespace osu.Game.Rulesets.S2VX.Replays {
    public class S2VXAutoGenerator : AutoGenerator {
        protected Replay Replay;
        protected List<ReplayFrame> Frames => Replay.Frames;

        public new Beatmap<S2VXHitObject> Beatmap => (Beatmap<S2VXHitObject>)base.Beatmap;

        public S2VXAutoGenerator(IBeatmap beatmap)
            : base(beatmap) => Replay = new Replay();

        public override Replay Generate() {
            Frames.Add(new S2VXReplayFrame());

            foreach (var hitObject in Beatmap.HitObjects) {
                Frames.Add(new S2VXReplayFrame {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                    // todo: add required inputs and extra frames.
                });
            }

            return Replay;
        }
    }
}

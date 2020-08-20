// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Replays;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.S2VX.Replays {
    public class S2VXReplayFrame : ReplayFrame {
        public List<S2VXAction> Actions { get; private set; } = new List<S2VXAction>();
        public void SetActions(List<S2VXAction> value) => Actions = value;

        public Vector2 Position { get; set; }

        public S2VXReplayFrame(S2VXAction? button = null) {
            if (button.HasValue) {
                Actions.Add(button.Value);
            }
        }
    }
}

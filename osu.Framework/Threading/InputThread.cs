﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Statistics;
using System.Collections.Generic;
using osu.Framework.Development;

namespace osu.Framework.Threading
{
    public class InputThread : GameThread
    {
        public InputThread()
            : base(name: "Input")
        {
        }

        internal override IEnumerable<StatisticsCounterType> StatisticsCounters => new[]
        {
            StatisticsCounterType.MouseEvents,
            StatisticsCounterType.KeyEvents,
            StatisticsCounterType.JoystickEvents,
            StatisticsCounterType.MidiEvents,
        };

        public override bool IsCurrent => ThreadSafety.IsInputThread;

        internal sealed override void MakeCurrent()
        {
            base.MakeCurrent();

            ThreadSafety.IsInputThread = true;
        }

        public override void Start()
        {
            // InputThread does not get started. it is run manually by GameHost.
        }
    }
}

﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Game.Tests;
using System;

namespace osu.Game.Rulesets.S2VX.Tests {
    public static class VisualTestRunner {
        [STAThread]
        public static int Main(string[] _) {
            using var host = Host.GetSuitableHost(@"osu", true);
            host.Run(new OsuTestBrowser());
            return 0;
        }
    }
}

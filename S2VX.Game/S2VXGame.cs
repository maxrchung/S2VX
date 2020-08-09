﻿using Microsoft.WindowsAPICodePack.Dialogs;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Editor;
using System;
using System.Collections.Generic;

namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private readonly S2VXEditor Editor = new S2VXEditor();

        [BackgroundDependencyLoader]
        private void Load() => Child = Editor;
    }
}

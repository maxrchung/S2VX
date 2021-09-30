![image](https://user-images.githubusercontent.com/3955187/135382794-9bdc5441-e29b-4a1f-bda8-2ecedcd9159f.png)


# S2VX

S2VX is a rhythm game where you click the squares to the beat. It is heavily inspired by [osu!](https://osu.ppy.sh) and is built with [osu!framework](https://github.com/ppy/osu-framework). S2VX uses a timeable, movable, scalable, rotatable camera system to place notes, bringing you more interactive gameplay and motion sickness.

Check out our website and some demos: https://S2VX.com

# How 2 Download

For Windows users, you can follow these instructions to run the game:

1. Download `S2VX.zip` from our latest release: https://github.com/maxrchung/S2VX/releases/latest/download/S2VX.zip
2. Extract the zip contents to a location.
3. Run `S2VX.exe`.

Alternatively, you can build and run the S2VX source code yourself:

1. Install the correct SDK for [.NET](https://dotnet.microsoft.com). You can find the version we're using by checking the `<TargetFramework>` tag in one of our `csproj` file: https://github.com/maxrchung/S2VX/blob/master/S2VX.Desktop/S2VX.Desktop.csproj#L3

2. Clone the repository:

```bash
git clone https://github.com/maxrchung/S2VX.git
```

3. Navigate to the `S2VX.Desktop` project:

```bash
cd S2VX.Desktop
```

4. Start the project:

```bash
dotnet run
```

# How 2 Play

S2VX plays very similarly to osu!standard. Notes appear on the screen and your goal is to hit them as accurately as possible. In S2VX, there are regular notes as well as hold notes. Regular notes only need a single click to register a score, while hold notes need a button held down for a duration. Score is calculated based on hit error, so the lower the score the better.

You can use any of these inputs to hit notes:
* `LClick`
* `RClick`
* `Z`
* `X`
* `C`
* `V`
* `A`
* `S`
* `D`
* `F`

# How 2 Edit

You can drag an mp3 file into the song selection screen to create a new story, then you can edit it in the S2VX editor. The editor lets you place notes, manipulate the camera, add commands, among other things. You can view all the available tools and hotkeys by going through the top menu. Here are a few key features we support:

### Tools
* Select Tool: `LClick` to select and move notes
* Note Tool: `LClick` to place regular notes
* Hold Note Tool: `LClick` to place hold notes, `RClick` to end hold note
* Camera Tool: `LClick` to move camera, `RClick` to rotate camera, `MClick` to scale camera

### Command Panel

Use the Command Panel in the editor to create, update, and delete commands. A command has a start time, start value, end time, end value, and customizes the story's behavior. Here are a few examples of what you can do:

* `NotesAlpha|0|0.7|0|0.7|`: Set the transparency of regular notes to 0.7 at the start of the story.
* `HoldNotesOutlineColor|0|(1,1,1)|0|(1,1,1)`: Set the hold note outline color to white at the start of the story.
* `NotesShowTime|0|800|0|800`: Set the note show time to 800 milliseconds at the start of the story. A note's life time determined by fade in time, show time, and fade out time. The note should be hit at the end of the note's show time.
* `TimingChange|0|242|368002|242|`: Set the editor BPM to 242 from time 0 to 368002 milliseconds. This has no visible impact in gameplay but makes it much easier to place notes in the editor.

# How 2 Thank

Thanks to the other members of the S2VX team for making this project possible:
* [Xenocidel](https://github.com/xenocidel)
* [naranja-sagged](https://github.com/naranja-sagged)

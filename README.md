![image](https://user-images.githubusercontent.com/3955187/135382794-9bdc5441-e29b-4a1f-bda8-2ecedcd9159f.png)


# S2VX

S2VX is a rhythm game where you click the squares to the beat. It is heavily inspired by [osu!](https://osu.ppy.sh) and is built with [osu!framework](https://github.com/ppy/osu-framework). S2VX uses a timeable, movable, scalable, rotatable camera system to place notes, enabling some more interactive gameplay and motion sickness.

Check out our website and some demos: https://S2VX.com

# How 2 Download

For Windows users, you can follow these instructions to run the game:

1. Download `S2VX.zip` from our latest release: https://github.com/maxrchung/S2VX/releases/latest/download/S2VX.zip
2. Extract the zip contents to a location.
3. Run `S2VX.exe`.

Alternatively, you can build and run the S2VX source code yourself:

1. Install the correct [SDK for .NET](https://dotnet.microsoft.com). You can find the version we're using by checking the `<TargetFramework>` tag in one of our `csproj` file: https://github.com/maxrchung/S2VX/blob/master/S2VX.Desktop/S2VX.Desktop.csproj#L3

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

<img src="https://user-images.githubusercontent.com/3955187/135947355-b3c50f29-e7d7-4b69-bcad-4c8d9a03338b.png" width="666" />

S2VX plays very similarly to osu!standard. Notes appear on the screen and your goal is to hit them as accurately as possible. In S2VX, there are regular notes as well as hold notes. Regular notes only need a single click to register a score, while hold notes need a button held down for a duration. **Please note that hold notes do not need to be tracked with the cursor or released at the end.** Score is calculated based on hit error, so the lower the score the better.

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

<img src="https://user-images.githubusercontent.com/3955187/135947872-e5cb8a74-1044-43c9-8b6f-111aa561d906.png" width="666" />

You can drag an mp3 file into the song selection screen to create a new story, then you can edit it in the S2VX editor. The editor lets you place notes, manipulate the camera, add commands, among other things. **Protip: When editing, finalize your camera movements first before placing notes! If you ever need to make changes, it'll be a pain to try to move all your notes.** You can view all the available tools and hotkeys by going through the top menu. Here are a few key features we support:

### Tools
* Select Tool: `LClick` to select and move notes.
* Note Tool: `LClick` to place regular notes.
* Hold Note Tool: `LClick` to place hold notes and add anchor/waypoints, `RClick` to end hold note. Hold notes always progress at a constant speed.
* Camera Tool: `LClick` to move camera, `RClick` to rotate camera, `MClick` to scale camera.

### Command Panel

<img src="https://user-images.githubusercontent.com/3955187/135948393-8af71640-6c18-40be-8c2b-c0d446800251.png" width="666" />

Use the Command Panel in the editor to create, update, and delete commands. A command customizes a story's behavior with a start time, start value, end time, and end value. Here are a few examples of what commands can do:

* `NotesAlpha|0|0.7|0|0.7|` Sets the transparency of regular notes to 0.7 at the start of the story.
* `HoldNotesOutlineColor|0|(1,1,1)|0|(1,1,1)` Sets the hold note outline color to white at the start of the story.
* `NotesShowTime|0|800|0|800` Sets the note show time to 800 milliseconds at the start of the story. A note's visibility is determined by fade in time, show time, and fade out time. S2VX will show the note so that it should be hit at the end of the note's show time. For example, a note is placed at time 1000 and has fade in time 100, show time 800, and fade out time 100. The note will fade in at time 100-200, stay shown at time 200-1000, and fade out at time 1000-1100.
* `TimingChange|0|242|368002|242|` Sets the editor BPM to 242 from time 0 to 368002 milliseconds. This has no visible impact in gameplay but makes it much easier to place notes in the editor.

# How 2 Thank

Thanks to the other members of the S2VX team for making this project possible:
* [Xenocidel](https://github.com/xenocidel)
* [naranja-sagged](https://github.com/naranja-sagged)

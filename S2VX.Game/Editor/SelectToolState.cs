﻿using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Extensions.Color4Extensions;

namespace S2VX.Game.Editor {
    public class SelectToolState : ToolState {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        [Resolved]
        private S2VXEditor Editor { get; set; } = null;
        private Dictionary<double, Note> TimeToSelectedNote { get; set; } = new Dictionary<double, Note>();
        private Dictionary<Drawable, Note> NoteSelectionToNote { get; set; } = new Dictionary<Drawable, Note>();

        private const float SelectionIndicatorThickness = 0.025f;

        public override bool OnToolClick(ClickEvent _) {
            TimeToSelectedNote.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(_.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBar);
            Console.WriteLine(mousePos);
            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                var leftBound = notes.Value.DrawPosition.X - notes.Value.DrawSize.X / 2;
                var rightBound = notes.Value.DrawPosition.X + notes.Value.DrawSize.X / 2;
                var topBound = notes.Value.DrawPosition.Y - notes.Value.DrawSize.Y / 2;
                var bottomBound = notes.Value.DrawPosition.Y + notes.Value.DrawSize.Y / 2;

                var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
                var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
                if (mouseInXRange && mouseInYRange) {
                    TimeToSelectedNote[notes.Key.EndTime] = notes.Key;
                    var noteSelection = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.5f),
                        Width = notes.Key.Size.X + SelectionIndicatorThickness,
                        Height = notes.Key.Size.Y + SelectionIndicatorThickness,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = notes.Key.Rotation,
                    };
                    noteSelection.X = notes.Key.Position.X;
                    noteSelection.Y = notes.Key.Position.Y;
                    Editor.NoteSelectionIndicators.Add(noteSelection);
                    NoteSelectionToNote[noteSelection] = notes.Key;
                    //Console.WriteLine(notes.Key.EndTime);
                    Console.WriteLine($"Actual Note Coords: {notes.Key.DrawPosition}");
                    return false;
                }
            }
            return false;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        public override void HandleExit() {
            Editor.NotesTimeline.TickBar.RemoveAll(item => item.Name == "TimelineSelection");
            Editor.NoteSelectionIndicators.Clear();
        }

        protected override void Update() {
            Editor.NotesTimeline.TickBar.RemoveAll(item => item.Name == "TimelineSelection");
            var lowerBound = Story.GameTime - Editor.NotesTimeline.SectionLength * 1000 / 2;
            var upperBound = Story.GameTime + Editor.NotesTimeline.SectionLength * 1000 / 2;
            foreach (var timeAndNote in TimeToSelectedNote) {
                if (lowerBound <= timeAndNote.Key && timeAndNote.Key <= upperBound) {
                    var relativePosition = (timeAndNote.Key - lowerBound) / (Editor.NotesTimeline.SectionLength * 1000);
                    var indication = new RelativeBox {
                        Name = "TimelineSelection",
                        Colour = Color4.LimeGreen.Opacity(0.727f),
                        Width = NotesTimeline.TimelineNoteWidth + 0.009727f,
                        Height = NotesTimeline.TimelineNoteHeight + 0.1f,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                    };
                    Editor.NotesTimeline.TickBar.Add(indication);
                    //Console.WriteLine(indication.DrawPosition);
                }
            }
            foreach (var noteSelection in Editor.NoteSelectionIndicators) {
                noteSelection.Rotation = NoteSelectionToNote[noteSelection].Rotation;
                noteSelection.X = NoteSelectionToNote[noteSelection].Position.X;
                noteSelection.Y = NoteSelectionToNote[noteSelection].Position.Y;
            }
        }
    }
}
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleCameraToolActions : IReversible {
        private EditorScreen Editor { get; set; }

        private CameraMoveCommand MoveCommand { get; set; }
        private CameraScaleCommand ScaleCommand { get; set; }
        private CameraRotateCommand RotateCommand { get; set; }

        public ReversibleCameraToolActions(
            EditorScreen editor,
            CameraMoveCommand moveCommand,
            CameraScaleCommand scaleCommand,
            CameraRotateCommand rotateCommand
        ) {
            Editor = editor;
            MoveCommand = moveCommand;
            ScaleCommand = scaleCommand;
            RotateCommand = rotateCommand;
        }

        public void Undo() {
            if (MoveCommand != null) {
                Editor.CommandPanel.RemoveCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Editor.CommandPanel.RemoveCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Editor.CommandPanel.RemoveCommand(RotateCommand);
            }
        }

        public void Redo() {
            if (MoveCommand != null) {
                Editor.CommandPanel.AddCommand(MoveCommand);
            }
            if (ScaleCommand != null) {
                Editor.CommandPanel.AddCommand(ScaleCommand);
            }
            if (RotateCommand != null) {
                Editor.CommandPanel.AddCommand(RotateCommand);
            }
        }
    }
}

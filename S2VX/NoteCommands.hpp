#pragma once
#include "Command.hpp"
namespace S2VX {
	struct NoteCommand : Command {
		NoteCommand(int start, int end, float startX, float startY, float endX, float endY);

	};
}

#include "CursorCommand.hpp"
namespace S2VX {
	CursorCommand::CursorCommand(Cursor& pCursor, const int start, const int end, const EasingType easing)
		: Command{ start, end, easing },
		cursor{ pCursor } {}
}
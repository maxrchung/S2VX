#pragma once
#include "Command.hpp"
namespace S2VX {
	class Cursor;
	class CursorCommand : public Command {
	public:
		explicit CursorCommand(Cursor& pCursor, const int start, const int end, const EasingType easing);
		virtual void update(const float easing) = 0;
	protected:
		Cursor& cursor;
	};
}
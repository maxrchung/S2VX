#pragma once
#include "CursorCommand.hpp"
namespace S2VX {
	class CursorFeatherCommand : public CursorCommand {
	public:
		CursorFeatherCommand(Cursor& cursor, const int start, const int end, const EasingType easing, const float pStartFeather, const float pEndFeather);
		void update(const float easing);
	private:
		void validateCursorFeather(const float feather);
		const float endFeather;
		const float startFeather;
	};
}
#pragma once
#include "CursorCommand.hpp"
namespace S2VX {
	class Cursor;
	class CursorScaleCommand : public CursorCommand {
	public:
		CursorScaleCommand(Cursor* cursor, const int start, const int end, const EasingType easing, const float pStartScale, const float pEndScale);
		void update(const float easing);
	private:
		void validateCursorScale(const float scale);
		const float endScale;
		const float startScale;
	};
}
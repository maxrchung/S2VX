#pragma once
#include "CursorCommand.hpp"
namespace S2VX {
	class Cursor;
	class CursorFade : public CursorCommand {
	public:
		CursorFade(Cursor* cursor, int start, int end, EasingType easing, const float pStartFade, const float pEndFade);
		void update(const float easing);
	private:
		void validateCursorFade(const float fade);
		const float endFade;
		const float startFade;
	};
}
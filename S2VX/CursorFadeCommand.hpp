#pragma once
#include "CursorCommand.hpp"
namespace S2VX {
	class Cursor;
	class CursorFadeCommand : public CursorCommand {
	public:
		CursorFadeCommand(Cursor* cursor, const int start, const int end, const EasingType easing, const float pStartFade, const float pEndFade);
		void update(const float easing);
	private:
		void validateCursorFade(const float fade);
		const float endFade;
		const float startFade;
	};
}
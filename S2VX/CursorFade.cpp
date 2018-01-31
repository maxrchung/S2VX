#include "CursorFade.hpp"
#include "Cursor.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CursorFade::CursorFade(Cursor* cursor, int start, int end, EasingType easing, const float pStartFade, const float pEndFade)
		: CursorCommand{ cursor, start, end, easing },
		endFade{ pEndFade },
		startFade{ pStartFade } {
		validateCursorFade(endFade);
		validateCursorFade(startFade);
	}
	void CursorFade::update(const float easing) {
		const auto fade = glm::mix(startFade, endFade, easing);
		cursor->setFade(fade);
	}
	void CursorFade::validateCursorFade(const float fade) {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Cursor fade must be >= 0 and <= 1. Given: " + std::to_string(fade));
		}
	}
}
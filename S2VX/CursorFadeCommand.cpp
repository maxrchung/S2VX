#include "CursorFadeCommand.hpp"
#include "Cursor.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CursorFadeCommand::CursorFadeCommand(Cursor* cursor, const int start, const int end, const EasingType easing, const float pStartFade, const float pEndFade)
		: CursorCommand{ cursor, start, end, easing },
		endFade{ pEndFade },
		startFade{ pStartFade } {
		validateCursorFade(endFade);
		validateCursorFade(startFade);
	}
	void CursorFadeCommand::update(const float easing) {
		const auto fade = glm::mix(startFade, endFade, easing);
		cursor->setFade(fade);
	}
	void CursorFadeCommand::validateCursorFade(const float fade) {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Cursor fade must be >= 0 and <= 1. Given: " + std::to_string(fade));
		}
	}
}
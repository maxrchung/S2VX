#include "CursorScale.hpp"
#include "Cursor.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	CursorScale::CursorScale(Cursor* cursor, const int start, const int end, EasingType easing, const float pStartScale, const float pEndScale)
		: CursorCommand{ cursor, start, end, easing },
		startScale{ pStartScale },
		endScale{ pEndScale } {
		validateCursorScale(startScale);
		validateCursorScale(endScale);
	}
	void CursorScale::update(const float easing) {
		const auto scale = glm::mix(startScale, endScale, easing);
		cursor->setScale(scale);
	}
	void CursorScale::validateCursorScale(const float scale) {
		if (scale < 0.0f) {
			throw ScriptError("Sprite scale must be >= 0 and <= 1. Given: " + std::to_string(scale));
		}
	}
}
#include "CursorColor.hpp"
#include "Cursor.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	CursorColor::CursorColor(Cursor* cursor, const int start, const int end, EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB)
		: CursorCommand{ cursor, start, end, easing },
		endColor{ glm::vec3{ endR, endG, endB } },
		startColor{ glm::vec3{ startR, startG, startB } } {
		validateCursorColor(endColor);
		validateCursorColor(startColor);
	}
	void CursorColor::update(const float easing) {
		const auto color = glm::mix(startColor, endColor, easing);
		cursor->setColor(color);
	}
	void CursorColor::validateCursorColor(const glm::vec3& color) {
		if (color.r < 0.0f || color.r > 1.0f ||
			color.g < 0.0f || color.g > 1.0f ||
			color.b < 0.0f || color.b > 1.0f) {
			auto message = std::stringstream();
			message << "Cursor color values must be >= 0 and <= 1. Given: ("
				<< std::to_string(color.r) << ','
				<< std::to_string(color.g) << ','
				<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
	}
}
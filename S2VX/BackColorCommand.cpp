#include "BackColorCommand.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	BackColorCommand::BackColorCommand(Back* const pBack, const int start, const int end, const EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB)
		: BackCommand{ pBack, start, end, easing },
		startColor{ startR, startG, startB },
		endColor{ endR, endG, endB } {
		validateBackColor(startColor);
		validateBackColor(endColor);
	}
	void BackColorCommand::validateBackColor(const glm::vec3& color) const {
		if (color.r < 0.0f || color.r > 1.0f ||
			color.g < 0.0f || color.g > 1.0f ||
			color.b < 0.0f || color.b > 1.0f) {
			auto message = std::stringstream();
			message << "Back color values must be >= 0 and <= 1. Given: ("
				<< std::to_string(color.r) << ','
				<< std::to_string(color.g) << ','
				<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
	}
	void BackColorCommand::update(const int time) {
		const auto interpolation = static_cast<float>(time - getStart()) / (getEnd() - getStart());
		const auto easing = Easing(getEasing(), interpolation);
		color = glm::mix(derived->startColor, derived->endColor, easing);

	}
}
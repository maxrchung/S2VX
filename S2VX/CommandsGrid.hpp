#pragma once 

#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class CommandGridColorBack : public Command {
	public:
		CommandGridColorBack(const Time& start, const Time& end, EasingType easing, float startR, float startG, float startA, float startB, float endR, float endG, float endB, float endA);
		glm::vec4 startColor;
		glm::vec4 endColor;
	};
}
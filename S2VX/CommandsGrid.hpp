#pragma once 

#include "Command.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class CommandGridColorBack : public Command {
	public:
		CommandGridColorBack(EasingType easing, const Time& start, const Time& end, float startR, float startG, float startA, float startB, float endR, float endG, float endB, float endA);
		glm::vec4 startColor;
		glm::vec4 endColor;
	};
}
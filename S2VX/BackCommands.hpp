#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct BackColorCommand : Command {
		BackColorCommand(int start, int end, EasingType pEasing, float startR, float startG, float startA, float startB, float endR, float endG, float endB, float endA)
			: Command{ CommandType::BackColor, start, end },
			easing{ pEasing },
			startColor{ startR, startG, startB, startA },
			endColor{ endR, endG, endB, endA } {}
		EasingType easing;
		glm::vec4 startColor;
		glm::vec4 endColor;
	};
}
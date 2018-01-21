#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct BackColorCommand : Command {
		explicit BackColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		const EasingType easing;
		const glm::vec3 startColor;
		const glm::vec3 endColor;
	};
}
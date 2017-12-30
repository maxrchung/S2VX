#pragma once

#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class CommandSpriteMove : public Command {
	public:
		CommandSpriteMove(const Time& start, const Time& end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY);
		EasingType easing;
		int spriteID;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};
}
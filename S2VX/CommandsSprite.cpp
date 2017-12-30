#include "CommandsSprite.hpp"

namespace S2VX {
	CommandSpriteMove::CommandSpriteMove(const Time& start, const Time& end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY)
		: Command{ CommandType::CommandSpriteMove, ElementType::Sprite, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}
}
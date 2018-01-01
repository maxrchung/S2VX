#include "SpriteCommands.hpp"
namespace S2VX {
	SpriteBindCommand::SpriteBindCommand(int pSpriteID, const std::string& pPath)
		: Command{ CommandType::SpriteBind, ElementType::Sprite, Time(), Time() },
		spriteID{ pSpriteID },
		path{ pPath } {}
	SpriteCreateCommand::SpriteCreateCommand(const Time& start, int pSpriteID)
		: Command{ CommandType::SpriteBind, ElementType::Sprite, start, start },
		spriteID{ pSpriteID } {}
	SpriteDeleteCommand::SpriteDeleteCommand(const Time& end, int pSpriteID)
		: Command{ CommandType::SpriteBind, ElementType::Sprite, end, end },
		spriteID{ pSpriteID } {}
	SpriteMoveCommand::SpriteMoveCommand(const Time& start, const Time& end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY)
		: Command{ CommandType::SpriteMove, ElementType::Sprite, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}
}
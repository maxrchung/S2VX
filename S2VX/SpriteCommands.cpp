#include "SpriteCommands.hpp"
namespace S2VX {
	SpriteBindCommand::SpriteBindCommand(int pSpriteID, const std::string& pPath)
		: Command{ CommandType::SpriteBind, 0, 0 },
		spriteID{ pSpriteID },
		path{ pPath } {}
	SpriteCreateCommand::SpriteCreateCommand(int start, int pSpriteID)
		: Command{ CommandType::SpriteCreate, start, start },
		spriteID{ pSpriteID } {}
	SpriteDeleteCommand::SpriteDeleteCommand(int end, int pSpriteID)
		: Command{ CommandType::SpriteDelete, end, end },
		spriteID{ pSpriteID } {}
	SpriteMoveCommand::SpriteMoveCommand(int start, int end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY)
		: Command{ CommandType::SpriteMove, start, end },
		easing{ pEasing },
		spriteID{ pSpriteID },
		startCoordinate{ glm::vec2(startX, startY) },
		endCoordinate{ glm::vec2(endX, endY) } {}
}
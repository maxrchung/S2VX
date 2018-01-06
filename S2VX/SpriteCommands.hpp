#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	struct SpriteBindCommand : Command {
		SpriteBindCommand(int pSpriteID, const std::string& pPath)
			: Command{ CommandType::SpriteBind, 0, 0 },
			spriteID{ pSpriteID },
			path{ pPath } {}
		int spriteID;
		std::string path;
	};
	struct SpriteCreateCommand : Command {
		SpriteCreateCommand(int start, int pSpriteID)
			: Command{ CommandType::SpriteCreate, start, start },
			spriteID{ pSpriteID } {}
		int spriteID;
	};
	struct SpriteDeleteCommand : Command {
		SpriteDeleteCommand(int end, int pSpriteID)
			: Command{ CommandType::SpriteDelete, end, end },
			spriteID{ pSpriteID } {}
		int spriteID;
	};
	struct SpriteMoveCommand : Command {
		SpriteMoveCommand(int start, int end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY)
			: Command{ CommandType::SpriteMove, start, end },
			easing{ pEasing },
			spriteID{ pSpriteID },
			startCoordinate{ glm::vec2(startX, startY) },
			endCoordinate{ glm::vec2(endX, endY) } {}
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
		int spriteID;
	};
}
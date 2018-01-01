#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	struct SpriteBindCommand : Command {
		SpriteBindCommand(int pSpriteID, const std::string& pPath);
		int spriteID;
		std::string path;
	};
	struct SpriteCreateCommand : Command {
		SpriteCreateCommand(const Time& start, int spriteID);
		int spriteID;
	};
	struct SpriteDeleteCommand : Command {
		SpriteDeleteCommand(const Time& end, int spriteID);
		int spriteID;
	};
	struct SpriteMoveCommand : Command {
		SpriteMoveCommand(const Time& start, const Time& end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY);
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
		int spriteID;
	};
}
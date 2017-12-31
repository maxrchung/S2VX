#pragma once

#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	class SpriteBindCommand : public Command {
	public:
		SpriteBindCommand(int pSpriteID, const std::string& pPath);
		int spriteID;
		std::string path;
	};

	class SpriteCreateCommand : public Command {
	public:
		SpriteCreateCommand(const Time& start, int spriteID);
		int spriteID;
	};

	class SpriteDeleteCommand : public Command {
	public:
		SpriteDeleteCommand(const Time& end, int spriteID);
		int spriteID;
	};

	class SpriteMoveCommand : public Command {
	public:
		SpriteMoveCommand(const Time& start, const Time& end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY);
		EasingType easing;
		int spriteID;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
	};
}
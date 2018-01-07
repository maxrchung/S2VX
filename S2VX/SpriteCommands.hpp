#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
#include <string>
namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	struct SpriteBindCommand : Command {
		SpriteBindCommand(int pSpriteID, const std::string& pPath);
		int spriteID;
		std::string path;
	};
	struct SpriteCreateCommand : Command {
		SpriteCreateCommand(int start, int pSpriteID);
		int spriteID;
	};
	struct SpriteDeleteCommand : Command {
		SpriteDeleteCommand(int end, int pSpriteID);
		int spriteID;
	};
	struct SpriteFadeCommand : Command {
		SpriteFadeCommand::SpriteFadeCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartFade, float pEndFade);
		EasingType easing;
		float startFade;
		float endFade;
		int spriteID;
	};
	struct SpriteMoveCommand : Command {
		SpriteMoveCommand(int start, int end, EasingType pEasing, int pSpriteID, float startX, float startY, float endX, float endY);
		EasingType easing;
		glm::vec2 startCoordinate;
		glm::vec2 endCoordinate;
		int spriteID;
	};
	struct SpriteRotateCommand : Command {
		SpriteRotateCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartRotation, float pEndRotation);
		EasingType easing;
		float startRotation;
		float endRotation;
		int spriteID;
	};
	struct SpriteScaleCommand : Command {
		SpriteScaleCommand(int start, int end, EasingType pEasing, int pSpriteID, float pStartScaleX, float pStartScaleY, float pEndScaleX, float pEndScaleY);
		EasingType easing;
		glm::vec2 startScale;
		glm::vec2 endScale;
		int spriteID;
	};
}
#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
#include <string>
namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	struct SpriteBindCommand : Command {
		SpriteBindCommand(const std::string& pPath);
		std::string path;
	};
	struct SpriteFadeCommand : Command {
		SpriteFadeCommand::SpriteFadeCommand(int start, int end, EasingType pEasing, float pStartFade, float pEndFade);
		EasingType easing;
		float startFade;
		float endFade;
	};
	struct SpriteMoveXCommand : Command {
		SpriteMoveXCommand(int start, int end, EasingType pEasing, int pStartX, int pEndX);
		EasingType easing;
		int startX;
		int endX;
		int spriteID;
	};
	struct SpriteMoveYCommand : Command {
		SpriteMoveYCommand(int start, int end, EasingType pEasing, int pStartY, int pEndY);
		EasingType easing;
		int startY;
		int endY;
	};
	struct SpriteRotateCommand : Command {
		SpriteRotateCommand(int start, int end, EasingType pEasing, float pStartRotation, float pEndRotation);
		EasingType easing;
		float startRotation;
		float endRotation;
	};
	struct SpriteScaleCommand : Command {
		SpriteScaleCommand(int start, int end, EasingType pEasing, float pStartScaleX, float pStartScaleY, float pEndScaleX, float pEndScaleY);
		EasingType easing;
		glm::vec2 startScale;
		glm::vec2 endScale;
	};
}
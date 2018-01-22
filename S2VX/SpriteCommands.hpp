#pragma once
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
#include <string>
namespace S2VX {
	// Special command that only takes in path and spriteID; put into a lookup later to determine sprite path
	struct SpriteBindCommand : Command {
		explicit SpriteBindCommand(const std::string& pPath);
		const std::string path;
	};
	struct SpriteColorCommand : Command {
		explicit SpriteColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		const EasingType easing;
		const glm::vec3 startColor;
		const glm::vec3 endColor;
	};
	struct SpriteFadeCommand : Command {
		explicit SpriteFadeCommand::SpriteFadeCommand(const int start, const int end, const EasingType pEasing, const float pStartFade, const float pEndFade);
		const EasingType easing;
		const float startFade;
		const float endFade;
	};
	struct SpriteMoveCommand : Command {
		explicit SpriteMoveCommand(const int start, const int end, const EasingType pEasing, const int pStartX, const int pStartY, const int pEndX, const int pEndY);
		const EasingType easing;
		const glm::vec2 startCoordinate;
		const glm::vec2 endCoordinate;
	};
	struct SpriteRotateCommand : Command {
		explicit SpriteRotateCommand(const int start, const int end, const EasingType pEasing, const float pStartRotation, const float pEndRotation);
		const EasingType easing;
		const float startRotation;
		const float endRotation;
	};
	struct SpriteScaleCommand : Command {
		explicit SpriteScaleCommand(const int start, const int end, const EasingType pEasing, const float pStartScaleX, const float pStartScaleY, const float pEndScaleX, const float pEndScaleY);
		const EasingType easing;
		const glm::vec2 startScale;
		const glm::vec2 endScale;
	};
}
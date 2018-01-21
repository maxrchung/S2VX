#pragma once 
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct GridColorCommand : Command {
		explicit GridColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		const EasingType easing;
		const glm::vec3 startColor;
		const glm::vec3 endColor;
	};
	struct GridFadeCommand : Command {
		explicit GridFadeCommand(const int start, const int end, const EasingType pEasing, const float pStartFade, const float pEndFade);
		const EasingType easing;
		const float startFade;
		const float endFade;
	};
	struct GridFeatherCommand : Command {
		explicit GridFeatherCommand(const int start, const int end, const EasingType pEasing, const float pStartFeather, const float pEndFeather);
		const EasingType easing;
		const float startFeather;
		const float endFeather;
	};
	struct GridThicknessCommand : Command {
		explicit GridThicknessCommand(const int start, const int end, const EasingType pEasing, const float pStartThickness, const float pEndThickness);
		const EasingType easing;
		const float startThickness;
		const float endThickness;
	};
}
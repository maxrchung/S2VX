#pragma once 
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct GridSetLineWidthCommand : Command {
		GridSetLineWidthCommand(const Time& start, const Time& end, EasingType pEasing, float pStartThickness, float pEndThickness)
			: Command{ CommandType::GridSetLineWidth, ElementType::Grid, start, end },
			easing{ pEasing },
			startThickness{ pStartThickness },
			endThickness{ pEndThickness } {}
		EasingType easing;
		float startThickness;
		float endThickness;
	};
}
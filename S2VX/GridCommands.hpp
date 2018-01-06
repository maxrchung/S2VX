#pragma once 
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct GridFeatherCommand : Command {
		GridFeatherCommand(int start, int end, EasingType pEasing, float pStartFeather, float pEndFeather)
			: Command{ CommandType::GridFeather, start, end },
			easing{ pEasing },
			startFeather{ pStartFeather },
			endFeather{ pEndFeather } {}
		EasingType easing;
		float startFeather;
		float endFeather;
	};
	struct GridThicknessCommand : Command {
		GridThicknessCommand(int start, int end, EasingType pEasing, float pStartThickness, float pEndThickness)
			: Command{ CommandType::GridThickness, start, end },
			easing{ pEasing },
			startThickness{ pStartThickness },
			endThickness{ pEndThickness } {}
		EasingType easing;
		float startThickness;
		float endThickness;
	};
}
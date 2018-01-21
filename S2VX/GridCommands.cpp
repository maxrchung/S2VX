#include "GridCommands.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	GridColorCommand::GridColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB)
		: Command{ CommandType::GridColor, start, end },
		easing{ pEasing },
		startColor{ startR, startG, startB },
		endColor{ endR, endG, endB } {
		validateColor(startColor);
		validateColor(endColor);
	}
	GridFadeCommand::GridFadeCommand(const int start, const int end, const EasingType pEasing, const float pStartFade, const float pEndFade)
		: Command{ CommandType::GridFade, start, end },
		easing{ pEasing },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateFade(startFade);
		validateFade(endFade);
	}
	GridFeatherCommand::GridFeatherCommand(const int start, const int end, const EasingType pEasing, const float pStartFeather, const float pEndFeather)
		: Command{ CommandType::GridFeather, start, end },
		easing{ pEasing },
		startFeather{ pStartFeather },
		endFeather{ pEndFeather } {
		validateFeather(startFeather);
		validateFeather(endFeather);
	}
	GridThicknessCommand::GridThicknessCommand(const int start, const int end, const EasingType pEasing, const float pStartThickness, const float pEndThickness)
		: Command{ CommandType::GridThickness, start, end },
		easing{ pEasing },
		startThickness{ pStartThickness },
		endThickness{ pEndThickness } {
		validateThickness(startThickness);
		validateThickness(endThickness);
	}
}
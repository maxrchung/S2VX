#include "GridCommands.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	GridFeatherCommand::GridFeatherCommand(int start, int end, EasingType pEasing, float pStartFeather, float pEndFeather)
		: Command{ CommandType::GridFeather, start, end },
		easing{ pEasing },
		startFeather{ pStartFeather },
		endFeather{ pEndFeather } {
		validateFeather(startFeather);
		validateFeather(endFeather);
	}
	void GridFeatherCommand::validateFeather(float feather) {
		if (feather < 0.0f) {
			throw ScriptError("Grid line feather must be greater than or equal to 0. Given: " + std::to_string(feather));
		}
	}
	GridThicknessCommand::GridThicknessCommand(int start, int end, EasingType pEasing, float pStartThickness, float pEndThickness)
		: Command{ CommandType::GridThickness, start, end },
		easing{ pEasing },
		startThickness{ pStartThickness },
		endThickness{ pEndThickness } {
		validateThickness(startThickness);
		validateThickness(endThickness);
	}
	void GridThicknessCommand::validateThickness(float thickness) {
		if (thickness < 0.0f) {
			throw ScriptError("Grid line thickness must be greater than equal to 0. Given: " + std::to_string(thickness));
		}
	}
}
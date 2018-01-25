#include "GridFeatherCommand.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	GridFeatherCommand::GridFeatherCommand(Grid* const grid, const int start, const int end, const EasingType easing, const float pStartFeather, const float pEndFeather)
		: GridCommand{ grid, start, end, easing },
		startFeather{ pStartFeather },
		endFeather{ pEndFeather } {
		validateGridFeather(startFeather);
		validateGridFeather(endFeather);
	}
	void GridFeatherCommand::validateGridFeather(const float feather) const {
		if (feather < 0.0f) {
			throw ScriptError("Grid line feather must be >= 0. Given: " + std::to_string(feather));
		}
	}
	void GridFeatherCommand::update(const int time) {
	}
}
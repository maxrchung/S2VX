#include "GridFadeCommand.hpp"
#include "Grid.hpp"
#include "ScriptError.hpp"
namespace S2VX {
	GridFadeCommand::GridFadeCommand(Grid& grid, const int start, const int end, const EasingType easing, const float pStartFade, const float pEndFade)
		: GridCommand{ grid, start, end, easing },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateGridFade(startFade);
		validateGridFade(endFade);
	}
	void GridFadeCommand::validateGridFade(const float fade) const {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Grid fade must be >= 0 and <= 1. Given: " + std::to_string(fade));
		}
	}
	void GridFadeCommand::update(const float easing) {
		const auto fade = glm::mix(startFade, endFade, easing);
		grid.setFade(fade);
	}
}
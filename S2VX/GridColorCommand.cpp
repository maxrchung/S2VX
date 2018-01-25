#include "GridColorCommand.hpp"
#include "ScriptError.hpp"
#include <sstream>
namespace S2VX {
	GridColorCommand::GridColorCommand(Grid* const grid, const int start, const int end, const EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB)
		: GridCommand{ grid, start, end, easing },
		startColor{ startR, startG, startB },
		endColor{ endR, endG, endB } {
		validateGridColor(startColor);
		validateGridColor(endColor);
	}
	void GridColorCommand::validateGridColor(const glm::vec3& color) const {
		if (color.r < 0.0f || color.r > 1.0f ||
			color.g < 0.0f || color.g > 1.0f ||
			color.b < 0.0f || color.b > 1.0f) {
			auto message = std::stringstream();
			message << "Grid color values must be >= 0 and <= 1. Given: ("
				<< std::to_string(color.r) << ','
				<< std::to_string(color.g) << ','
				<< std::to_string(color.b) << ')';
			throw ScriptError(message.str());
		}
	}
	void GridColorCommand::update(const int time) {
	}
}
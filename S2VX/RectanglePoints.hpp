#pragma once
#include "RectangleOrientation.hpp"
#include <glm/glm.hpp>
#include <vector>
namespace S2VX {
	struct RectanglePoints {
		RectanglePoints(float x, float y, RectangleOrientation pOrientation);
		std::vector<float> points;
		RectangleOrientation orientation;
	};
}
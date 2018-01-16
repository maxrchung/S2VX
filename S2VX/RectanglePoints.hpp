#pragma once
#include "RectangleOrientation.hpp"
#include <glm/glm.hpp>
#include <vector>
namespace S2VX {
	class RectanglePoints {
	public:
		RectanglePoints(float x, float y, RectangleOrientation pOrientation);
		RectangleOrientation getOrientation() { return orientation; }
		std::vector<float> getPoints() { return points; }
		std::vector<float> getScaled(float pScale);
	private:
		RectangleOrientation orientation;
		std::vector<float> points;
		std::vector<float> scaled;
	};
}
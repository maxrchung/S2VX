#include "RectanglePoints.hpp"
namespace S2VX {
	RectanglePoints::RectanglePoints(float pX, float pY, RectangleOrientation pOrientation)
		: x{ pX }, y{ pY },
		points{
			// Top right triangle
			x, y,	 1.0f,	 1.0f,
			x, y,	 1.0f,	-1.0f,
			x, y,	-1.0f,	 1.0f,
			// Bot left triangle
			x, y,	 1.0f,	-1.0f,
			x, y,	-1.0f,	 1.0f,
			x, y,	-1.0f,	-1.0f
		 }, orientation{ pOrientation } {}
	std::vector<float> RectanglePoints::getScaled(const float scale) const {
		std::vector<float> scaled = {
			// Top right triangle
			x * scale, y * scale,	 1.0f,	 1.0f,
			x * scale, y * scale,	 1.0f,	-1.0f,
			x * scale, y * scale,	-1.0f,	 1.0f,
			// Bot left triangle
			x * scale, y * scale,	 1.0f,	-1.0f,
			x * scale, y * scale,	-1.0f,	 1.0f,
			x * scale, y * scale,	-1.0f,	-1.0f
		};
		return scaled;
	}
}
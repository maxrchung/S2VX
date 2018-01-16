#include "RectanglePoints.hpp"
namespace S2VX {
	RectanglePoints::RectanglePoints(float x, float y, RectangleOrientation pOrientation)
		: points{
			// Top right triangle
			x, y,	 1.0f,	 1.0f,
			x, y,	 1.0f,	-1.0f,
			x, y,	-1.0f,	 1.0f,
			// Bot left triangle
			x, y,	 1.0f,	-1.0f,
			x, y,	-1.0f,	 1.0f,
			x, y,	-1.0f,	-1.0f
		  }, scaled{ points }, orientation{ pOrientation } {}
	std::vector<float> RectanglePoints::getScaled(float scale) {
		for (int i = 0; i < static_cast<int>(points.size());) {
			scaled[i] = points[i] * scale;
			if (i % 4 == 2) {
				i += 2;
			}
			else {
				++i;
			}
		}
		return scaled;
	}
}
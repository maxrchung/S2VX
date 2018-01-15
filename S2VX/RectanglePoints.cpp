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
		  }, orientation{ pOrientation } {}
}
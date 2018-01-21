#pragma once
#include "RectangleOrientation.hpp"
#include <glm/glm.hpp>
#include <vector>
namespace S2VX {
	class RectanglePoints {
	public:
		explicit RectanglePoints(const float pX, const float pY, const RectangleOrientation pOrientation);
		RectangleOrientation getOrientation() const { return orientation; }
		const std::vector<float>& getPoints() const { return points; }
		std::vector<float> getScaled(const float pScale) const;
	private:
		const float x;
		const float y;
		const RectangleOrientation orientation;
		const std::vector<float> points;
	};
}
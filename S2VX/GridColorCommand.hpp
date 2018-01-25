#pragma once
#include "GridCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class GridColorCommand : public GridCommand {
	public:
		explicit GridColorCommand(Grid* const grid, const int start, const int end, const EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void update(const int time);
	private:
		void validateGridColor(const glm::vec3& color) const;
		const glm::vec3 endColor;
		const glm::vec3 startColor;
	};
}
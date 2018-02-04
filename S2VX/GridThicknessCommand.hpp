#pragma once
#include "GridCommand.hpp"
namespace S2VX {
	class GridThicknessCommand : public GridCommand {
	public:
		explicit GridThicknessCommand(Grid& grid, const int start, const int end, const EasingType easing, const float pStartThickness, const float pEndThickness);
		void update(const float easing);
	private:
		void validateGridThickness(const float thickness) const;
		const float endThickness;
		const float startThickness;
	};
}
#pragma once 
#include "Command.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Grid;
	class GridCommand : public Command {
	public:
		explicit GridCommand(Grid* const pGrid, const int start, const int end, const EasingType easing);
		virtual void update(const float easing) = 0;
	protected:
		Grid* const grid;
	};
}
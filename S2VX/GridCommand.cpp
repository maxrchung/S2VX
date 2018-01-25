#include "GridCommand.hpp"
namespace S2VX {
	GridCommand::GridCommand(Grid* const pGrid, const int start, const int end, const EasingType easing)
		: Command{start, end, easing},
		grid{ pGrid } {}
}
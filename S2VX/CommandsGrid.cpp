#include "CommandsGrid.hpp"

namespace S2VX {
	CommandGridColorBack::CommandGridColorBack(EasingType easing, const Time& start, const Time& end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA)
		: Command{ CommandType::CommandGridColorBack, ElementType::Grid, easing, start, end },
		endColor{ endR, endG, endB, endA } {
	}
}
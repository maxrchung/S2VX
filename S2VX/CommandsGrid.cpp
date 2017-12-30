#include "CommandsGrid.hpp"

namespace S2VX {
	CommandGridColorBack::CommandGridColorBack(const Time& start, const Time& end, EasingType pEasing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA)
		: Command{ CommandType::CommandGridColorBack, ElementType::Grid, start, end},
		easing{ pEasing },
		startColor{ startR, startG, startB, startA },
		endColor{ endR, endG, endB, endA } {
	}
}
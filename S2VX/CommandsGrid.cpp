#include "CommandsGrid.hpp"

namespace S2VX {
	CommandGridColorBack::CommandGridColorBack(const Time& start, const Time& end, EasingType easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA)
		: Command{ CommandType::CommandGridColorBack, ElementType::Grid, easing, start, end},
		startColor{ startR, startG, startB, startA },
		endColor{ endR, endG, endB, endA } {
	}
}
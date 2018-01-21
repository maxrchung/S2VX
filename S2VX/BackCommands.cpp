#include "BackCommands.hpp"
namespace S2VX {
	BackColorCommand::BackColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startA, const float startB, const float endR, const float endG, const float endB, const float endA)
		: Command{ CommandType::BackColor, start, end },
		easing{ pEasing },
		startColor{ startR, startG, startB, startA },
		endColor{ endR, endG, endB, endA } {
		validateColor(startColor);
		validateColor(endColor);
	}
}
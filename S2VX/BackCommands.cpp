#include "BackCommands.hpp"
namespace S2VX {
	BackColorCommand::BackColorCommand(int start, int end, EasingType pEasing, float startR, float startG, float startA, float startB, float endR, float endG, float endB, float endA)
		: Command{ CommandType::BackColor, start, end },
		easing{ pEasing },
		startColor{ startR, startG, startB, startA },
		endColor{ endR, endG, endB, endA } {
		validateColor(startColor);
		validateColor(endColor);
	}
}
#include "BackCommands.hpp"
namespace S2VX {
	BackColorCommand::BackColorCommand(const int start, const int end, const EasingType pEasing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB)
		: Command{ CommandType::BackColor, start, end },
		easing{ pEasing },
		startColor{ startR, startG, startB},
		endColor{ endR, endG, endB } {
		validateColor(startColor);
		validateColor(endColor);
	}
}
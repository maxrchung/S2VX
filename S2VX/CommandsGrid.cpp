#include "CommandsGrid.hpp"

CommandGridColorBack::CommandGridColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA)
	: Command{ CommandType::CommandGridColorBack, ElementType::Grid, start, end },
	startColor{	startR, startG, startB, startA },
	endColor{ endR, endG, endB, endA } {}
#include "CommandsGrid.hpp"

CommandGrid_ColorBack::CommandGrid_ColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA)
	: Command{ CommandType::CommandGrid_ColorBack, ElementType::Grid, start, end },
	startColor{	startR, startG, startB, startA },
	endColor{ endR, endG, endB, endA } {}
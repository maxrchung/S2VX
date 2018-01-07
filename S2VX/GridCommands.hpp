#pragma once 
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct GridFeatherCommand : Command {
	public:
		GridFeatherCommand(int start, int end, EasingType pEasing, float pStartFeather, float pEndFeather);
		EasingType easing;
		float startFeather;
		float endFeather;
	private:
		void validateFeather(float feather);
	};
	struct GridThicknessCommand : Command {
	public:
		GridThicknessCommand(int start, int end, EasingType pEasing, float pStartThickness, float pEndThickness);
		EasingType easing;
		float startThickness;
		float endThickness;
	private:
		void validateThickness(float thickness);
	};
}
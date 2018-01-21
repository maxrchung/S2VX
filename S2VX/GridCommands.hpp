#pragma once 
#include "Command.hpp"
#include "EasingType.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	struct GridFeatherCommand : Command {
	public:
		explicit GridFeatherCommand(const int start, const int end, const EasingType pEasing, const float pStartFeather, const float pEndFeather);
		const EasingType easing;
		const float startFeather;
		const float endFeather;
	private:
		void validateFeather(float feather) const;
	};
	struct GridThicknessCommand : Command {
	public:
		explicit GridThicknessCommand(const int start, const int end, const EasingType pEasing, const float pStartThickness, const float pEndThickness);
		const EasingType easing;
		const float startThickness;
		const float endThickness;
	private:
		void validateThickness(float thickness) const;
	};
}
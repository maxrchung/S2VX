#pragma once
#include "GridCommand.hpp"
namespace S2VX {
	class GridFeatherCommand : public GridCommand {
	public:
		explicit GridFeatherCommand(Grid& grid, const int start, const int end, const EasingType easing, const float pStartFeather, const float pEndFeather);
		void update(const float easing);
	private:
		void validateGridFeather(const float feather) const;
		const float endFeather;
		const float startFeather;
	};
}
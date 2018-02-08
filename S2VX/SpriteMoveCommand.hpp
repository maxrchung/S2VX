#pragma once
#include "SpriteCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class SpriteMoveCommand : public SpriteCommand {
	public:
		explicit SpriteMoveCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const int pStartX, const int pStartY, const int pEndX, const int pEndY);
		void update(const float easing);
		const glm::vec2& getEndCoordinate() const { return endCoordinate; }
		const glm::vec2& getStartCoordinate() const { return startCoordinate; }
	private:
		const glm::vec2 endCoordinate;
		const glm::vec2 startCoordinate;
	};
}
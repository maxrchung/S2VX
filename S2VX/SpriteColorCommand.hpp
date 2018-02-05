#pragma once
#include "SpriteCommand.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class SpriteColorCommand : public SpriteCommand {
	public:
		explicit SpriteColorCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void update(const float easing);
	private:
		void validateSpriteColor(const glm::vec3& color) const;
		const glm::vec3 endColor;
		const glm::vec3 startColor;
	};
}
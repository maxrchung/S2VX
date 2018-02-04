#include "SpriteRotateCommand.hpp"
#include "Sprite.hpp"
namespace S2VX {
	SpriteRotateCommand::SpriteRotateCommand(Sprite& sprite, const int start, const int end, const EasingType easing, const float pStartRotation, const float pEndRotation)
		: SpriteCommand{ sprite, start, end, easing },
		startRotation{ pStartRotation },
		endRotation{ pEndRotation } {}
	void SpriteRotateCommand::update(const float easing) {
		const auto rotation = glm::mix(startRotation, endRotation, easing);
		sprite.setRotation(rotation);
	}
}
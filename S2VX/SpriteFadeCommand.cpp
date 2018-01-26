#include "SpriteFadeCommand.hpp"
#include "ScriptError.hpp"
#include "Sprite.hpp"
namespace S2VX {
	SpriteFadeCommand::SpriteFadeCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartFade, const float pEndFade)
		: SpriteCommand{ sprite, start, end, easing },
		startFade{ pStartFade },
		endFade{ pEndFade } {
		validateSpriteFade(startFade);
		validateSpriteFade(endFade);
	}
	void SpriteFadeCommand::validateSpriteFade(const float fade) const {
		if (fade < 0.0f || fade > 1.0f) {
			throw ScriptError("Sprite fade must be >= 0 and <= 1. Given: " + std::to_string(fade));
		}
	}
	void SpriteFadeCommand::update(const float easing) {
		const auto fade = glm::mix(startFade, endFade, easing);
		sprite->setFade(fade);
	}
}
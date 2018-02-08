#pragma once
#include "SpriteCommand.hpp"
namespace S2VX {
	class SpriteFadeCommand : public SpriteCommand {
	public:
		explicit SpriteFadeCommand(Sprite* const sprite, const int start, const int end, const EasingType easing, const float pStartFade, const float pEndFade);
		void update(const float easing);
		const float getEndFade() const { return endFade; }
		const float getStartFade() const { return startFade; }
	private:
		void validateSpriteFade(const float fade) const;
		const float endFade;
		const float startFade;
	};
}
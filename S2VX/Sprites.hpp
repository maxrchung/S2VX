#pragma once
#include "Element.hpp"
#include "Sprite.hpp"
#include "SpriteComparison.hpp"
namespace S2VX {
	class Sprites : public Element {
	public:
		Sprites() {};
		void addSprite(Sprite&& sprite);
		// Used during Scripting to set last sprite
		Sprite& getLastSprite();
		void draw();
		void update(const int time);
		void sort();
	private:
		SpriteComparison comparison;
		std::vector<Sprite> sprites;
	};
}
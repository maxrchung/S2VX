#pragma once
#include "Element.hpp"
#include "Sprite.hpp"
#include "SpriteUniquePointerComparison.hpp"
namespace S2VX {
	class Sprites : public Element {
	public:
		Sprites() {};
		void addSprite(std::unique_ptr<Sprite>&& sprite);
		// Used during Scripting to set last sprite
		Sprite* const getLastSprite();
		void draw();
		void update(const int time);
		void sort();
	private:
		SpriteUniquePointerComparison comparison;
		std::vector<std::unique_ptr<Sprite>> sprites;
	};
}
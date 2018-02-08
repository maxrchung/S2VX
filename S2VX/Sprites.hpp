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
		std::vector<std::unique_ptr<Sprite>>& getSprites() { return sprites; }
		void draw();
		void update(const int time);
		void sort();
	private:
		static const SpriteUniquePointerComparison spriteComparison;
		// In a perfect world I'd avoid pointers, but SpriteCommand 
		// keeps a Sprite reference that can be invalidated
		std::vector<std::unique_ptr<Sprite>> sprites;
	};
}
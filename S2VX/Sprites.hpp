#pragma once
#include "Element.hpp"
namespace S2VX {
	class Sprite;
	class Sprites : public Element {
	public:
		explicit Sprites(const std::vector<Sprite*>& pSprites);
		void draw(const Camera& camera);
		void update(const int time);
	private:
		std::vector<Sprite*> sprites;
	};
}
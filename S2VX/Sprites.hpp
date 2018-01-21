#pragma once
#include "Element.hpp"
#include "Sprite.hpp"
#include <map>
#include <unordered_map>
namespace S2VX {
	class Sprites : public Element {
	public:
		explicit Sprites(const std::vector<Sprite*>& pSprites);
		void draw(const Camera& camera);
		void update(const int time);
		void updateActives(const int time);
	private:
		std::vector<Sprite*> sprites;
	};
}
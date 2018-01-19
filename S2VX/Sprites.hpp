#pragma once
#include "Element.hpp"
#include "Sprite.hpp"
#include <map>
#include <unordered_map>
namespace S2VX {
	class Sprites : public Element {
	public:
		Sprites(const std::vector<Sprite*>& pSprites);
		void draw(const Camera& camera);
		void update(int time);
		void updateActives(int time);
	private:
		std::vector<Sprite*> sprites;
	};
}
#pragma once

#include "Element.hpp"
#include "Sprite.hpp"
#include "Texture.hpp"
#include <map>
#include <unordered_map>

namespace S2VX {
	class Sprites : public Element {
	public:
		Sprites(const std::vector<Command*>& commands);
		void update(const Time& time);
		void draw(Camera* camera);

		// Look up Texture
		std::unordered_map<std::string, std::unique_ptr<Texture>> textures;

		std::map<int, std::unique_ptr<Sprite>> sprites;
	};
}
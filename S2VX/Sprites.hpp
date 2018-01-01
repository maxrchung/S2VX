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
		void draw(Camera* camera);
		void update(const Time& time);
		// ID to Sprite
		std::map<int, Sprite> activeSprites;
		// ID to path
		std::unordered_map<int, std::string> paths;
		// path to Texture
		std::unordered_map<std::string, Texture> textures;
	};
}
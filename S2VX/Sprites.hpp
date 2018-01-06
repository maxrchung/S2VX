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
		void draw(const Camera& camera);
		void update(int time);
	private:
		std::unique_ptr<Shader> imageShader = std::make_unique<Shader>("Image.VertexShader", "Image.FragmentShader");
		// ID to Sprite
		// Pointer because destructor cleans up OpenGL objects
		std::map<int, std::unique_ptr<Sprite>> activeSprites;
		// ID to path
		std::unordered_map<int, std::string> paths;
		// path to Texture
		// Pointer because destructor cleans up OpenGL objects
		std::unordered_map<std::string, std::unique_ptr<Texture>> textures;
	};
}
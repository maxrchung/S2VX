#pragma once
#include "Texture.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Sprite {
	public:
		Sprite() {};
		Sprite(const Texture& pTexture);
		void draw();
		void move(glm::vec2 position);
	private:
		Texture texture;
	};
}
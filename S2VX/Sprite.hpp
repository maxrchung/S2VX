#pragma once
#include "Texture.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Sprite {
	public:
		Sprite() {};
		~Sprite();
		Sprite(const Texture& pTexture);
		void draw();
		void move(glm::vec2 position);
	private:
		Texture texture;
		unsigned int imageVertexArray;
		unsigned int imageVertexBuffer;
		unsigned int imageElementBuffer;
		static constexpr float corners[16] = {
			// Position			// Texture
			-1.0f,	-1.0f,		-1.0f,	-1.0f,
			-1.0f,	 1.0f,		-1.0f,	 1.0f,
			 1.0f,	-1.0f,		 1.0f,	-1.0f,
			 1.0f,	 1.0f,		 1.0f,	 1.0f
		};
		static constexpr int cornerIndices[16] = {
			0, 1, 3,
			1, 2, 3
		};
	};
}
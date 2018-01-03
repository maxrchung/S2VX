#pragma once
#include "Camera.hpp"
#include "Shader.hpp"
#include "Texture.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Sprite {
	public:
		Sprite() {};
		// Cleanup OpenGL objects
		~Sprite();
		Sprite(Texture* pTexture, Shader* imageShader);
		void draw(const Camera& camera);
		void move(glm::vec2 pPosition);
	private:
		Shader* imageShader;
		static constexpr float corners[16] = {
			// Position			// Texture
			 0.5f,	 0.5f,		1.0f,	1.0f, // TR
			 0.5f,	-0.5f,		1.0f,	0.0f, // BR
			-0.5f,	-0.5f,		0.0f,	0.0f, // BL
			-0.5f,	 0.5f,		0.0f,	1.0f  // TL
		};
		static constexpr unsigned int cornerIndices[16] = {
			0, 1, 3,
			1, 2, 3
		};
		Texture* texture;
		unsigned int imageVertexArray;
		unsigned int imageVertexBuffer;
		unsigned int imageElementBuffer;
		glm::vec2 position;
	};
}
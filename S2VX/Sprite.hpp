#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
#include <memory>
namespace S2VX {
	class Shader;
	class Texture;
	class Sprite : public Element{
	public:
		explicit Sprite(Camera& pCamera, const Texture& pTexture, Shader& pShader);
		// Cleanup OpenGL objects
		~Sprite();
		const int getEnd() const { return end; }
		const int getStart() const { return start; }
		void draw();
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setPosition(const glm::vec2 pPosition) { position = pPosition; }
		void setRotation(const float pRotation) { rotation = pRotation; }
		void setScale(const glm::vec2& pScale) { scale = pScale; }
		// Sprite has a custom sort because to set its start and end values
		void sort();
	private:
		Camera& camera;
		const Texture& texture;
		float fade;
		float rotation;
		glm::vec2 position;
		glm::vec2 scale;
		glm::vec3 color;
		int start;
		int end;
		Shader& shader;
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
		unsigned int vertexArray;
		unsigned int vertexBuffer;
		unsigned int elementBuffer;
	};
}
#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
#include <memory>
namespace S2VX {
	class Shader;
	class Texture;
	class Sprite : public Element{
	public:
		explicit Sprite(const Texture* const pTexture, Shader* const pImageShader);
		// Cleanup OpenGL objects
		~Sprite();
		const int getEnd() const { return start; }
		const int getStart() const { return end; }
		void draw(const Camera& camera);
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setPosition(const glm::vec2 pPosition) { position = pPosition; }
		void setRotation(const float pRotation) { rotation = pRotation; }
		void setScale(const glm::vec2& pScale) { scale = pScale; }
		// Sprite has a custom sort because to set its start and end values
		void sort();
	private:
		int start;
		int end;
		const Texture* const texture;
		glm::vec3 color;
		float fade;
		float rotation;
		glm::vec2 position;
		glm::vec2 scale;
		Shader* const imageShader;
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
		unsigned int imageVertexArray;
		unsigned int imageVertexBuffer;
		unsigned int imageElementBuffer;
	};
}
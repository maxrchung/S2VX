#pragma once
#include "Camera.hpp"
#include <glm/glm.hpp>
#include <memory>
namespace S2VX {
	class Shader;
	class Texture;
	class Sprite : public Element{
	public:
		explicit Sprite() {};
		explicit Sprite(const std::vector<Command*> pCommands, const Texture* const pTexture, Shader* const pImageShader);
		// Cleanup OpenGL objects
		~Sprite();
		void draw(const Camera& camera);
		int getEnd() const { return end; }
		int getStart() const { return start; }
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setPosition(const glm::vec2 pPosition) { position = pPosition; }
		void setRotation(const float pRotation) { rotation = pRotation; }
		void setScale(const glm::vec2& pScale) { scale = pScale; }
	private:
		const Texture* const texture = nullptr;
		glm::vec3 color = glm::vec3{ 1.0f, 1.0f, 1.0f };
		float fade = 1.0f;
		float rotation = 0.0f;
		glm::vec2 position = glm::vec2{ 0 };
		glm::vec2 scale = glm::vec2{ 1.0f, 1.0f };
		int start;
		int end;
		Shader* const imageShader = nullptr;
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
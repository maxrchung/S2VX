#pragma once
#include "Camera.hpp"
#include "Shader.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Texture;
	class Sprite : public Element{
	public:
		Sprite(const std::vector<Command*> pCommands, Texture* pTexture, Shader* pImageShader);
		// Cleanup OpenGL objects
		~Sprite();
		void draw(const Camera& camera);
		int getEnd() const { return end; }
		int getStart() const { return start; }
		void setFade(float pFade) { fade = pFade; }
		void setPositionX(float posX) { position.x = posX; }
		void setPositionY(float posY) { position.y = posY; }
		void setRotation(float pRotation) { rotation = pRotation; }
		void setScale(const glm::vec2& pScale) { scale = pScale; }
		void update(int time);
	private:
		float fade = 1.0f;
		float rotation = 0.0f;
		glm::vec2 position = glm::vec2(0);
		glm::vec2 scale = glm::vec2(1.0f, 1.0f);
		int start;
		int end;
		std::unique_ptr<Shader> imageShader;
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
	};
	class SpriteUniquePointerComparison {
	public:
		// Sort by start time
		bool operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs);
	};
}
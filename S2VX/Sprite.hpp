#pragma once
#include "Camera.hpp"
#include "Shader.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Texture;
	class Sprite : public Element{
	public:
		explicit Sprite(const std::vector<Command*> pCommands, const Texture* const pTexture, Shader* const pImageShader);
		// Cleanup OpenGL objects
		~Sprite();
		void draw(const Camera& camera);
		int getEnd() const { return end; }
		int getStart() const { return start; }
		void setFade(const float pFade) { fade = pFade; }
		void setPosition(const glm::vec2 pPosition) { position = pPosition; }
		void setRotation(const float pRotation) { rotation = pRotation; }
		void setScale(const glm::vec2& pScale) { scale = pScale; }
		void update(const int time);
	private:
		const Texture* const texture;
		float fade = 1.0f;
		float rotation = 0.0f;
		glm::vec2 position = glm::vec2(0);
		glm::vec2 scale = glm::vec2(1.0f, 1.0f);
		int start;
		int end;
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
	struct SpriteUniquePointerComparison {
		// Sort by start time
		bool operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs) const;
	};
}
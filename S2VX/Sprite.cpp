#include "Sprite.hpp"
#include <glad/glad.h>
namespace S2VX {
	Sprite::Sprite(const Texture& pTexture)
		: texture(pTexture) {
		glGenVertexArrays(1, &imageVertexArray);
		glGenBuffers(1, &imageVertexBuffer);
		glGenBuffers(1, &imageElementBuffer);
		glBindVertexArray(imageVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, imageVertexBuffer);
		glBufferData(GL_ARRAY_BUFFER, sizeof(corners), corners, GL_DYNAMIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, imageElementBuffer);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(cornerIndices), cornerIndices, GL_STATIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
	}
	Sprite::~Sprite() {
		glDeleteVertexArrays(1, &imageVertexArray);
		glDeleteBuffers(1, &imageVertexBuffer);
		glDeleteBuffers(1, &imageElementBuffer);
	}
	void Sprite::draw() {
	}
	void Sprite::move(glm::vec2 position) {
	}
}
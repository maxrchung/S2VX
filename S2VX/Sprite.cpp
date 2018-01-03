#include "Sprite.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
namespace S2VX {
	Sprite::Sprite(const Texture& pTexture)
		: texture(pTexture) {
		glGenVertexArrays(1, &imageVertexArray);
		glGenBuffers(1, &imageVertexBuffer);
		glGenBuffers(1, &imageElementBuffer);
		glBindVertexArray(imageVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, imageVertexBuffer);
		glBufferData(GL_ARRAY_BUFFER, sizeof(corners), corners, GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, imageElementBuffer);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(cornerIndices), cornerIndices, GL_STATIC_DRAW);
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		imageShader.use();
		imageShader.setInt("image", 0);
	}
	Sprite::~Sprite() {
		//glDeleteVertexArrays(1, &imageVertexArray);
		//glDeleteBuffers(1, &imageVertexBuffer);
		//glDeleteBuffers(1, &imageElementBuffer);
	}
	void Sprite::draw(const Camera& camera) {
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, texture.getImageTexture());
		imageShader.use();
		glm::mat4 model;
		model = glm::translate(model, glm::vec3(position.x, position.y, 0.0f));
		imageShader.setMat4("model", model);
		imageShader.setMat4("view", camera.getView());
		imageShader.setMat4("projection", camera.getProjection());
		imageShader.use();
		glBindVertexArray(imageVertexArray);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	}
	void Sprite::move(glm::vec2 pPosition) {
		position = pPosition;
	}
}
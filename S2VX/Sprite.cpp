#include "Sprite.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Sprite::Sprite(Texture* pTexture, Shader* pImageShader)
		: texture{ pTexture }, imageShader{ pImageShader } {
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
		imageShader->use();
		imageShader->setInt("image", 0);
	}
	Sprite::~Sprite() {
		glDeleteVertexArrays(1, &imageVertexArray);
		glDeleteBuffers(1, &imageVertexBuffer);
		glDeleteBuffers(1, &imageElementBuffer);
	}
	void Sprite::draw(const Camera& camera) {
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, texture->getImageTexture());
		imageShader->use();
		glm::mat4 model;
		model = glm::translate(model, glm::vec3(position.x, position.y, 0.0f));
		model = glm::scale(model, glm::vec3(scale, 1.0f));
		model = glm::rotate(model, glm::radians(rotation), glm::vec3(0.0f, 0.0f, 1.0f));
		imageShader->setMat4("model", model);
		imageShader->setMat4("view", camera.getView());
		imageShader->setMat4("projection", camera.getProjection());
		imageShader->setFloat("fade", fade);
		imageShader->use();
		glBindVertexArray(imageVertexArray);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	}
	void Sprite::setFade(float pFade) {
		fade = pFade;
	}
	void Sprite::setPositionX(float posX) {
		position.x = posX;
	}
	void Sprite::setPositionY(float posY) {
		position.y = posY;
	}
	void Sprite::setRotation(float pRotation) {
		rotation = pRotation;
	}
	void Sprite::setScale(const glm::vec2& pScale) {
		scale = pScale;
	}
}
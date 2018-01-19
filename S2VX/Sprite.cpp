#include "Sprite.hpp"
#include "Easing.hpp"
#include "SpriteCommands.hpp"
#include "Texture.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Sprite::Sprite(const std::vector<Command*> pCommands, Texture* pTexture, Shader* pImageShader)
		: Element{ pCommands }, texture{ pTexture }, imageShader{ pImageShader } {
		int pStart = std::numeric_limits<int>::max();
		int pEnd = std::numeric_limits<int>::lowest();
		for (auto command : commands) {
			if (command->start < pStart) {
				pStart = command->start;
			}
			if (command->end > pEnd) {
				pEnd = command->end;
			}
		}
		start = pStart;
		end = pEnd;
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
		glBindVertexArray(imageVertexArray);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	}
	void Sprite::update(int time) {
		for (auto active : actives) {
			auto command = commands[active];
			auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::SpriteFade: {
					auto derived = static_cast<SpriteFadeCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto fade = glm::mix(derived->startFade, derived->endFade, easing);
					setFade(fade);
					break;
				}
				case CommandType::SpriteMoveX: {
					auto derived = static_cast<SpriteMoveXCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					// Need to cast to float or else glm::mix will return an int
					auto posX = glm::mix(static_cast<float>(derived->startX), static_cast<float>(derived->endX), easing);
					setPositionX(posX);
					break;
				}
				case CommandType::SpriteMoveY: {
					auto derived = static_cast<SpriteMoveYCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto posY = glm::mix(static_cast<float>(derived->startY), static_cast<float>(derived->endY), easing);
					setPositionY(posY);
					break;
				}
				case CommandType::SpriteRotate: {
					auto derived = static_cast<SpriteRotateCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto rotation = glm::mix(derived->startRotation, derived->endRotation, easing);
					setRotation(rotation);
					break;
				}
				case CommandType::SpriteScale: {
					auto derived = static_cast<SpriteScaleCommand*>(command);
					auto easing = Easing(derived->easing, interpolation);
					auto scale = glm::mix(derived->startScale, derived->endScale, easing);
					setScale(scale);
					break;
				}
			}
		}
	}
	bool SpriteUniquePointerComparison::operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs) {
		return lhs->getStart() < rhs->getStart();
	}
}
#include "Sprite.hpp"
#include "Easing.hpp"
#include "SpriteCommands.hpp"
#include "Texture.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Sprite::Sprite(const std::vector<Command*> pCommands, const Texture* const pTexture, Shader* const pImageShader)
		: Element{ pCommands }, texture{ pTexture }, imageShader{ pImageShader } {
		int pStart = std::numeric_limits<int>::max();
		int pEnd = std::numeric_limits<int>::lowest();
		for (const auto command : commands) {
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
		model = glm::translate(model, glm::vec3{ position.x, position.y, 0.0f });
		model = glm::scale(model, glm::vec3{ scale, 1.0f });
		model = glm::rotate(model, glm::radians(rotation), glm::vec3{ 0.0f, 0.0f, 1.0f });
		imageShader->setVec3("color", color);
		imageShader->setMat4("model", model);
		imageShader->setMat4("view", camera.getView());
		imageShader->setMat4("projection", camera.getProjection());
		imageShader->setFloat("fade", fade);
		glBindVertexArray(imageVertexArray);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
	}
	void Sprite::update(const int time) {
		for (const auto active : actives) {
			const auto command = commands[active];
			const auto interpolation = static_cast<float>(time - command->start) / (command->end - command->start);
			switch (command->commandType) {
				case CommandType::SpriteColor: {
					const auto derived = static_cast<SpriteColorCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto pColor = glm::mix(derived->startColor, derived->endColor, easing);
					color = pColor;
					break;
				}
				case CommandType::SpriteFade: {
					const auto derived = static_cast<SpriteFadeCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto fade = glm::mix(derived->startFade, derived->endFade, easing);
					setFade(fade);
					break;
				}
				case CommandType::SpriteMove: {
					const auto derived = static_cast<SpriteMoveCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					// Need to cast to float or else glm::mix will return an int
					const auto coordinate = glm::mix(derived->startCoordinate, derived->endCoordinate, easing);
					setPosition(coordinate);
					break;
				}
				case CommandType::SpriteRotate: {
					const auto derived = static_cast<SpriteRotateCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto rotation = glm::mix(derived->startRotation, derived->endRotation, easing);
					setRotation(rotation);
					break;
				}
				case CommandType::SpriteScale: {
					const auto derived = static_cast<SpriteScaleCommand*>(command);
					const auto easing = Easing(derived->easing, interpolation);
					const auto scale = glm::mix(derived->startScale, derived->endScale, easing);
					setScale(scale);
					break;
				}
			}
		}
	}
	bool SpriteUniquePointerComparison::operator() (const std::unique_ptr<Sprite>& lhs, const std::unique_ptr<Sprite>& rhs) const {
		return lhs->getStart() < rhs->getStart();
	}
}
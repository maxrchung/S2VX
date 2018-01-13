#include "Note.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Note::Note(const NoteConfiguration& pConfiguration) 
		: configuration{ pConfiguration }, adjustedPoints{ squarePoints } {
		glGenVertexArrays(1, &squareVertexArray);
		glGenBuffers(1, &squareVertexBuffer);
	}
	Note::~Note() {
		glDeleteVertexArrays(1, &squareVertexArray);
		glDeleteBuffers(1, &squareVertexBuffer);
	}
	void Note::draw(const Camera& camera) {
		glBindVertexArray(squareVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, squareVertexBuffer);
		auto lineWidth = configuration.getWidth();
		// Points need to be adjusted so that lines do not overlap
		// Right
		for (int i = 1; i < 24; i += 4) {
			if (adjustedPoints[i] > 0) {
				adjustedPoints[i] = squarePoints[i] + lineWidth - 0.01f;
			}
			else {
				adjustedPoints[i] = squarePoints[i] - lineWidth + 0.01f;
			}
		}
		// Bottom
		for (int i = 24; i < 48; i += 4) {
			if (adjustedPoints[i] > 0) {
				adjustedPoints[i] = squarePoints[i] + lineWidth - 0.01f;
			}
			else {
				adjustedPoints[i] = squarePoints[i] - lineWidth + 0.01f;
			}
		}
		// Left
		for (int i = 49; i < 72; i += 4) {
			if (adjustedPoints[i] > 0) {
				adjustedPoints[i] = squarePoints[i] + lineWidth - 0.01f;
			}
			else {
				adjustedPoints[i] = squarePoints[i] - lineWidth + 0.01f;
			}
		}
		// Top
		for (int i = 72; i < 96; i += 4) {
			if (adjustedPoints[i] > 0) {
				adjustedPoints[i] = squarePoints[i] + lineWidth - 0.01f;
			}
			else {
				adjustedPoints[i] = squarePoints[i] - lineWidth + 0.01f;
			}
		}
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * squarePointsLength, adjustedPoints.data(), GL_STATIC_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
		// Static square
		squareShader->use();
		glm::mat4 innerModel;
		innerModel = glm::translate(innerModel, glm::vec3(configuration.getPosition(), 0.0f));
		squareShader->setFloat("lineWidth", lineWidth);
		squareShader->setMat4("model", innerModel);
		squareShader->setMat4("view", camera.getView());
		squareShader->setMat4("projection", camera.getProjection());
		squareShader->setFloat("fade", 1.0f);
		squareShader->setFloat("feather", 0.01f);
		squareShader->use();
		glDrawArrays(GL_TRIANGLES, 0, squarePointsLength / 4);
		// Approaching square
		glm::mat4 outerModel;
		outerModel = glm::translate(outerModel, glm::vec3(configuration.getPosition(), 0.0f));
		// Only scale for outer square
		outerModel = glm::scale(outerModel, glm::vec3(activeScale, activeScale, 1.0f));
		squareShader->setMat4("model", outerModel);
		glDrawArrays(GL_TRIANGLES, 0, squarePointsLength / 4);

	}
	void Note::update(int time) {
		auto interpolation = static_cast<float>(time - configuration.getStart()) / (configuration.getEnd() - configuration.getStart());
		activeScale = glm::mix(configuration.getDistance(), 0.5f, interpolation);
	}
}
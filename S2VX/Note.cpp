#include "Note.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
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
	void Note::adjustPoints(int startIndex, int end, float lineWidth, float feather) {
		for (int i = startIndex; i < end; i += 4) {
			if (adjustedPoints[i] > 0) {
				adjustedPoints[i] = squarePoints[i] + lineWidth - feather / 2.0f;
			}
			else {
				adjustedPoints[i] = squarePoints[i] - lineWidth + feather / 2.0f;
			}
		}
	}
	void Note::draw(const Camera& camera) {
		glBindVertexArray(squareVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, squareVertexBuffer);
		auto lineWidth = configuration.getWidth();
		auto feather = configuration.getFeather();
		// Points need to be adjusted so that lines do not overlap
		adjustPoints( 1, 24, lineWidth, feather);
		//adjustPoints( 0, 24, feather, 0);
		adjustPoints(24, 48, lineWidth, feather);
		//adjustPoints(25, 48, -feather, 0);
		adjustPoints(49, 72, lineWidth, feather);
		//adjustPoints(48, 72, feather, 0);
		adjustPoints(72, 96, lineWidth, feather);
		//adjustPoints(73, 96, -feather, 0);
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * squarePointsLength, adjustedPoints.data(), GL_DYNAMIC_DRAW);
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
		squareShader->setFloat("feather", feather);
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
		auto start = configuration.getStart();
		auto end = configuration.getEnd();
		auto interpolation = static_cast<float>(time - configuration.getStart()) / (configuration.getEnd() - configuration.getStart());
		activeScale = glm::mix(configuration.getDistance(), 0.5f, interpolation);
		auto fadeIn = start + configuration.getFadeIn();
		auto fadeOut = end - configuration.getFadeOut();
		if (time < fadeIn) {
			interpolation = static_cast<float>(time - start) / (fadeIn - start);
			activeFade = glm::mix(0.0f, 1.0f, interpolation);
		}
		else if (time > fadeOut) {
			interpolation = static_cast<float>(time - fadeOut) / (end - fadeOut);
			activeFade = glm::mix(1.0f, 0.0f, interpolation);
		}
		else {
			activeFade = 1.0f;
		}
	}
}
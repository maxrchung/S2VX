#include "Note.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
namespace S2VX {
	Note::Note(const NoteConfiguration& pConfiguration) 
		: configuration{ pConfiguration },
		  lines{
			RectanglePoints(0.5f, 0.0f, RectangleOrientation::Vertical),
			RectanglePoints(0.0f, -0.5f, RectangleOrientation::Horizontal),
			RectanglePoints(-0.5f, 0.0f, RectangleOrientation::Vertical),
			RectanglePoints(0.0f, 0.5f, RectangleOrientation::Horizontal),
		  } {
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
		auto feather = configuration.getFeather();
		for (auto& line : lines) {
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * line.points.size(), line.points.data(), GL_DYNAMIC_DRAW);
			// Position
			glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);
			// Normal
			glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);
			// Static square
			squareShader->use();
			squareShader->setFloat("fade", 1.0f);
			squareShader->setFloat("feather", feather);
			if (line.orientation == RectangleOrientation::Vertical) {
				squareShader->setVec2("lengths", glm::vec2(lineWidth, 1.0f));
			}
			else {
				squareShader->setVec2("lengths", glm::vec2(1.0f, lineWidth));
			}
			glm::mat4 innerModel;
			innerModel = glm::translate(innerModel, glm::vec3(configuration.getPosition(), 0.0f));
			squareShader->setMat4("model", innerModel);
			squareShader->setMat4("projection", camera.getProjection());
			squareShader->setMat4("view", camera.getView());
			glDrawArrays(GL_TRIANGLES, 0, line.points.size() / 4);
			// Approaching square
			glm::mat4 outerModel;
			outerModel = glm::translate(outerModel, glm::vec3(configuration.getPosition(), 0.0f));
			// Only scale for outer square
			outerModel = glm::scale(outerModel, glm::vec3(activeScale, activeScale, 1.0f));
			squareShader->setMat4("model", outerModel);
			glDrawArrays(GL_TRIANGLES, 0, line.points.size() / 4);
		}
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
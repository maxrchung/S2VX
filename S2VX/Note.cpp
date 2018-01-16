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
		for (auto& line : lines) {
			auto points = line.getPoints();
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * points.size(), points.data(), GL_DYNAMIC_DRAW);
			// Position
			glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);
			// Normal
			glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);
			// Static square
			squareShader->use();
			squareShader->setFloat("fade", activeFade);
			squareShader->setFloat("feather", configuration.getFeather());
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				squareShader->setVec2("lengths", glm::vec2(lineWidth, 0.5f + lineWidth));
			}
			else {
				squareShader->setVec2("lengths", glm::vec2(0.5f + lineWidth, lineWidth));
			}
			glm::mat4 model;
			model = glm::translate(model, glm::vec3(configuration.getPosition(), 0.0f));
			squareShader->setMat4("model", model);
			squareShader->setMat4("projection", camera.getProjection());
			squareShader->setMat4("view", camera.getView());
			glDrawArrays(GL_TRIANGLES, 0, points.size() / 4);
			// Approaching square
			// Goal is to keep line width the same, so we scale the point positions but not the widths
			auto scaled = line.getScaled(activeScale);
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * scaled.size(), scaled.data(), GL_DYNAMIC_DRAW);
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				squareShader->setVec2("lengths", glm::vec2(lineWidth, 0.5f * activeScale + lineWidth));
			}
			else {
				squareShader->setVec2("lengths", glm::vec2(0.5f * activeScale + lineWidth, lineWidth));
			}
			glm::mat4 outerModel;
			glDrawArrays(GL_TRIANGLES, 0, scaled.size() / 4);
		}
	}
	void Note::update(int time) {
		auto start = configuration.getStart();
		auto end = configuration.getEnd();
		auto fadeIn = start + configuration.getFadeIn();
		auto fadeOut = end - configuration.getFadeOut();
		auto scaleInterpolation = static_cast<float>(time - configuration.getStart()) / (fadeOut - configuration.getStart());
		if (time >= fadeOut) {
			activeScale = 1.0f;
		}
		else {
			activeScale = glm::mix(configuration.getDistance(), 1.0f, scaleInterpolation);
		}
		auto fadeInterpolation = static_cast<float>(time - configuration.getStart()) / (configuration.getEnd() - configuration.getStart());
		if (time < fadeIn) {
			fadeInterpolation = static_cast<float>(time - start) / (fadeIn - start);
			activeFade = glm::mix(0.0f, 1.0f, fadeInterpolation);
		}
		else if (time > fadeOut) {
			fadeInterpolation = static_cast<float>(time - fadeOut) / (end - fadeOut);
			activeFade = glm::mix(1.0f, 0.0f, fadeInterpolation);
		}
		else {
			activeFade = 1.0f;
		}
	}
}
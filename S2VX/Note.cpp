#include "Note.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Note::Note(const NoteConfiguration& pConfiguration, Shader* pSquareShader)
		: configuration{ pConfiguration }, squareShader{ pSquareShader },
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
		const auto thickness = configuration.getThickness();
		for (const auto& line : lines) {
			const auto points = line.getPoints();
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * points.size(), points.data(), GL_DYNAMIC_DRAW);
			// Position
			glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
			glEnableVertexAttribArray(0);
			// Normal
			glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
			glEnableVertexAttribArray(1);
			// Static square
			squareShader->use();
			squareShader->setVec3("color", configuration.getColor());
			squareShader->setFloat("fade", fade);
			squareShader->setFloat("feather", configuration.getFeather());
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				squareShader->setVec2("lengths", glm::vec2{ thickness, 0.5f + thickness });
			}
			else {
				squareShader->setVec2("lengths", glm::vec2{ 0.5f + thickness, thickness });
			}
			glm::mat4 model;
			model = glm::translate(model, glm::vec3{ configuration.getPosition(), 0.0f });
			squareShader->setMat4("model", model);
			squareShader->setMat4("projection", camera.getProjection());
			squareShader->setMat4("view", camera.getView());
			glDrawArrays(GL_TRIANGLES, 0, points.size() / 4);
			// Approaching square
			// Goal is to keep line width the same, so we scale the point positions but not the widths
			const auto scaled = line.getScaled(scale);
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * scaled.size(), scaled.data(), GL_DYNAMIC_DRAW);
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				squareShader->setVec2("lengths", glm::vec2{ thickness, 0.5f * scale + thickness });
			}
			else {
				squareShader->setVec2("lengths", glm::vec2{ 0.5f * scale + thickness, thickness });
			}
			glDrawArrays(GL_TRIANGLES, 0, scaled.size() / 4);
		}
	}
	void Note::update(int time) {
		const auto start = configuration.getStart();
		const auto end = configuration.getEnd();
		const auto fadeIn = start + configuration.getFadeIn();
		const auto fadeOut = end - configuration.getFadeOut();
		const auto scaleInterpolation = static_cast<float>(time - configuration.getStart()) / (fadeOut - configuration.getStart());
		if (time >= fadeOut) {
			scale = 1.0f;
		}
		else {
			scale = glm::mix(configuration.getDistance(), 1.0f, scaleInterpolation);
		}
		if (time < fadeIn) {
			const auto fadeInterpolation = static_cast<float>(time - start) / (fadeIn - start);
			fade = glm::mix(0.0f, 1.0f, fadeInterpolation);
		}
		else if (time > fadeOut) {
			const auto fadeInterpolation = static_cast<float>(time - fadeOut) / (end - fadeOut);
			fade = glm::mix(1.0f, 0.0f, fadeInterpolation);
		}
		else {
			fade = 1.0f;
		}
	}
	bool NoteUniquePointerComparison::operator() (const std::unique_ptr<Note>& lhs, const std::unique_ptr<Note>& rhs) const {
		return lhs->getConfiguration().getStart() < rhs->getConfiguration().getStart();
	}
}
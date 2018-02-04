#include "Note.hpp"
#include "Camera.hpp"
#include "Shader.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	const std::array<RectanglePoints, 4>  Note::lines = { RectanglePoints(0.5f, 0.0f, RectangleOrientation::Vertical),
		RectanglePoints(0.0f, -0.5f, RectangleOrientation::Horizontal),
		RectanglePoints(-0.5f, 0.0f, RectangleOrientation::Vertical),
		RectanglePoints(0.0f, 0.5f, RectangleOrientation::Horizontal) };
	Note::Note(Camera& pCamera, const NoteConfiguration& pConfiguration, Shader& pShader)
		: camera{ pCamera },
		configuration{ pConfiguration }, 
		shader{ pShader } {
		glGenVertexArrays(1, &vertexArray);
		glGenBuffers(1, &vertexBuffer);
	}
	Note::~Note() {
		glDeleteVertexArrays(1, &vertexArray);
		glDeleteBuffers(1, &vertexBuffer);
	}
	Note::Note(Note&& other) 
		: camera{ other.camera },
		configuration{ other.configuration },
		fade{ other.fade },
		scale{ other.scale },
		shader{ other.shader },
		vertexArray{ other.vertexArray },
		vertexBuffer{ other.vertexBuffer } {
		other.vertexArray = 0;
		other.vertexBuffer = 0;
	}
	Note& Note::operator=(Note&& other) {
		if (this != &other) {
			fade = other.fade;
			scale = other.scale;
			vertexArray = other.vertexArray;
			vertexBuffer = other.vertexBuffer;
			other.vertexArray = 0;
			other.vertexBuffer = 0;
		}
		return *this;
	}
	void Note::draw() {
		glBindVertexArray(vertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
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
			shader.use();
			shader.setVec3("color", configuration.getColor());
			shader.setFloat("fade", fade);
			shader.setFloat("feather", configuration.getFeather());
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				shader.setVec2("thickness", glm::vec2{ thickness, 0.5f + thickness });
			}
			else {
				shader.setVec2("thickness", glm::vec2{ 0.5f + thickness, thickness });
			}
			glm::mat4 model;
			model = glm::translate(model, glm::vec3{ configuration.getPosition(), 0.0f });
			shader.setMat4("model", model);
			shader.setMat4("projection", camera.getProjection());
			shader.setMat4("view", camera.getView());
			glDrawArrays(GL_TRIANGLES, 0, points.size() / 4);
			// Approaching square
			// Goal is to keep line width the same, so we scale the point positions but not the widths
			const auto scaled = line.getScaled(scale);
			glBufferData(GL_ARRAY_BUFFER, sizeof(float) * scaled.size(), scaled.data(), GL_DYNAMIC_DRAW);
			if (line.getOrientation() == RectangleOrientation::Vertical) {
				shader.setVec2("thickness", glm::vec2{ thickness, 0.5f * scale + thickness });
			}
			else {
				shader.setVec2("thickness", glm::vec2{ 0.5f * scale + thickness, thickness });
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
			fade = glm::mix(0.0f, configuration.getFade(), fadeInterpolation);
		}
		else if (time > fadeOut) {
			const auto fadeInterpolation = static_cast<float>(time - fadeOut) / (end - fadeOut);
			fade = glm::mix(1.0f, configuration.getFade(), fadeInterpolation);
		}
		else {
			fade = configuration.getFade();
		}
	}
}
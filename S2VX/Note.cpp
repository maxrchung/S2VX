#include "Note.hpp"
#include <glad/glad.h>
#include <glm/gtc/matrix_transform.hpp>
namespace S2VX {
	Note::Note(const NoteConfiguration& pConfiguration) 
		: configuration{ pConfiguration } {
		glGenVertexArrays(1, &linesVertexArray);
		glGenBuffers(1, &linesVertexBuffer);
		glBindVertexArray(linesVertexArray);
		glBindBuffer(GL_ARRAY_BUFFER, linesVertexBuffer);
		// Use & to get pointer to first element
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * linePointsLength, linePoints, GL_STATIC_DRAW);
		// Position
		glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		// Normal
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)(2 * sizeof(float)));
		glEnableVertexAttribArray(1);
	}
	Note::~Note() {
		glDeleteVertexArrays(1, &linesVertexArray);
		glDeleteBuffers(1, &linesVertexBuffer);
	}
	void Note::draw(const Camera& camera) {
		glBindVertexArray(linesVertexArray);
		linesShader->use();
		glm::mat4 model;
		model = glm::translate(model, glm::vec3(configuration.getPosition(), 0.0f));
		model = glm::scale(model, glm::vec3(activeScale, activeScale, 1.0f));
		linesShader->setFloat("lineWidth", configuration.getWidth());
		linesShader->setMat4("model", model);
		linesShader->setMat4("view", camera.getView());
		linesShader->setMat4("projection", camera.getProjection());
		linesShader->setFloat("fade", 1.0f);
		linesShader->setFloat("feather", configuration.getFeather());
		linesShader->use();
		glDrawArrays(GL_TRIANGLES, 0, linePointsLength / 4);
	}
	void Note::update(int time) {
		auto interpolation = static_cast<float>(time - configuration.getStart()) / (configuration.getEnd() - configuration.getStart());
		activeScale = glm::mix(0.5f, configuration.getDistance(), interpolation);
	}
}
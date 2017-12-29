#pragma once

#include "Element.hpp"
#include "Shader.hpp"
#include <glm/glm.hpp>

namespace S2VX {
	class Grid : public Element {
	public:
		Grid(const std::vector<Command*>& commands);
		void update(const Time& time);
		void draw(Camera* camera);

		glm::vec4 backColor;

		unsigned int VBO, VAO;
		Shader shader;

		std::vector<float> vertices;

		void print(std::string id, glm::vec4 vector);
	};
}
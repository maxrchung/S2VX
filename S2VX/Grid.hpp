#pragma once
#include "Element.hpp"
#include "Shader.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Grid : public Element {
	public:
		Grid(const std::vector<Command*>& commands);
		void draw(const Camera& camera);
		void update(const Time& time);
	private:
		glm::vec4 backColor;
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
		Shader shader;
		std::vector<float> vertices;
	};
}
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
		Shader shader = Shader(R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.VertexShader)", R"(c:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.FragmentShader)");
		std::vector<float> linePoints;
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
	};
}
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
		Shader linesShader = Shader("Lines.VertexShader", "Lines.FragmentShader");
		std::vector<float> linePoints;
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
	};
}
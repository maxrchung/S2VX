#pragma once
#include "Element.hpp"
#include "Shader.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Grid : public Element {
	public:
		Grid(const std::vector<Command*>& commands);
		~Grid();
		void draw(const Camera& camera);
		void update(const Time& time);
	private:
		// https://blog.mapbox.com/drawing-antialiased-lines-with-opengl-8766f34192dc
		std::unique_ptr<Shader> linesShader = std::make_unique<Shader>("FeatherLines.VertexShader", "FeatherLines.FragmentShader");
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
		float lineWidth = 1.0f;
		float feather = 0.01f;
	};
}
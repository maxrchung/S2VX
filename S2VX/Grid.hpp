#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Shader;
	class Grid : public Element {
	public:
		explicit Grid(Shader* const lineShader);
		~Grid();
		void draw(const Camera& camera);
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setFeather(const float pFeather) { feather = pFeather; }
		void setThickness(const float pThickness) { thickness = pThickness; }
	private:
		// https://blog.mapbox.com/drawing-antialiased-lines-with-opengl-8766f34192dc
		float thickness;
		float feather;
		glm::vec3 color;
		float fade;
		Shader* const lineShader = nullptr;
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
	};
}
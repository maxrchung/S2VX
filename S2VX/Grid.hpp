#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Shader;
	class Grid : public Element {
	public:
		explicit Grid(const std::vector<Command*>& commands, Shader* const lineShader);
		~Grid();
		void draw(const Camera& camera);
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setFeather(const float pFeather) { feather = pFeather; }
		void setThickness(const float pThickness) { thickness = pThickness; }
	private:
		// https://blog.mapbox.com/drawing-antialiased-lines-with-opengl-8766f34192dc
		float thickness = 0.1f;
		float feather = 0.01f;
		glm::vec3 color = glm::vec3{ 1.0f, 1.0f, 1.0f };
		float fade = 1.0f;
		Shader* const lineShader;
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
	};
}
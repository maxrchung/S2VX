#pragma once
#include "Element.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Shader;
	class Grid : public Element {
	public:
		explicit Grid(Camera& pCamera, Shader& pShader);
		~Grid();
		Grid(Grid&& other);
		Grid& operator=(Grid&& other);
		void draw();
		void setColor(const glm::vec3& pColor) { color = pColor; }
		void setFade(const float pFade) { fade = pFade; }
		void setFeather(const float pFeather) { feather = pFeather; }
		void setThickness(const float pThickness) { thickness = pThickness; }
	private:
		Grid(const Grid&) = delete;
		Grid& operator=(const Grid&) = delete;
		Camera& camera;
		// https://blog.mapbox.com/drawing-antialiased-lines-with-opengl-8766f34192dc
		float fade;
		float feather;
		float thickness;
		glm::vec3 color;
		Shader& shader;
		unsigned int vertexArray;
		unsigned int vertexBuffer;
	};
}
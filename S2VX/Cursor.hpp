#pragma once
#include "Element.hpp"
#include "RectanglePoints.hpp"
#include <glm/glm.hpp>
namespace S2VX {
	class Display;
	class Shader;
	class Cursor : public Element {
	public:
		Cursor(const Camera& pCamera, const Display& pDisplay, Shader& pShader);
		~Cursor();
		Cursor(Cursor&& other);
		Cursor& operator=(Cursor&& other);
		void draw();
		void setFade(const float pFade) { fade = pFade; }
		void setFeather(const float pFeather) { feather = pFeather; }
		void setScale(const float pScale) { scale = pScale; }
		void setColor(const glm::vec3& pColor) { color = pColor; }
	private:
		Cursor(const Cursor&) = delete;
		Cursor& operator=(const Cursor&) = delete;
		const Camera& camera;
		const Display& display;
		float fade;
		float feather;
		float scale;
		glm::vec3 color;
		Shader& shader;
		static const RectanglePoints cursor;
		unsigned int vertexArray;
		unsigned int vertexBuffer;
	};
}
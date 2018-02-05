#include "Elements.hpp"
#include "Display.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements(const Display& display) 
		: cursorShader{ "Cursor.VertexShader", "Cursor.FragmentShader" },
		imageShader{ "Image.VertexShader", "Image.FragmentShader" },
		lineShader{ "Line.VertexShader", "Line.FragmentShader" },
		rectangleShader{ "Rectangle.VertexShader", "Rectangle.FragmentShader" },
		back{ Back() },
		camera{ Camera() },
		cursor{ Cursor(camera, display, cursorShader) },
		grid{ Grid(camera, lineShader) },
		notes{ Notes() },
		sprites{ Sprites() },
		all{ &camera, &back, &sprites, &grid, &notes, &cursor } {}
	void Elements::draw() {
		for (const auto element : all) {
			element->draw();
		}
	}
	void Elements::sort() {
		for (const auto element : all) {
			element->sort();
		}
	}
	void Elements::update(const int time) {
		for (const auto element : all) {
			element->update(time);
		}
	}
}
#include "Elements.hpp"
#include "Display.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements(const Display& display) 
		: cursorShader{ "Cursor.VertexShader", "Cursor.FragmentShader" },
		imageShader{ "Image.VertexShader", "Image.FragmentShader" },
		lineShader{ "Line.VertexShader", "Line.FragmentShader" },
		rectangleShader{ "Rectangle.VertexShader", "Rectangle.FragmentShader" },
		back{ std::make_unique<Back>() },
		camera{ std::make_unique<Camera>() },
		cursor{ std::make_unique<Cursor>(*camera.get(), display, cursorShader) },
		grid{ std::make_unique<Grid>(*camera.get(), lineShader) },
		notes{ std::make_unique<Notes>() },
		sprites{ std::make_unique<Sprites>() },
		all{ camera.get(), back.get(), sprites.get(), grid.get(), notes.get(), cursor.get() } {}
	void Elements::draw() {
		back->draw();
		sprites->draw();
		grid->draw();
		notes->draw();
		cursor->draw();
	}
	void Elements::sort() {
		for (auto& element : all) {
			element->sort();
		}
	}
	void Elements::update(const int time) {
		for (const auto element : all) {
			element->update(time);
		}
	}
}
#include "Elements.hpp"
#include "Display.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements(const Display& display) 
		: cursorShader{ std::make_unique<Shader>("Cursor.VertexShader", "Cursor.FragmentShader") },
		imageShader{ std::make_unique<Shader>("Image.VertexShader", "Image.FragmentShader") },
		lineShader{ std::make_unique<Shader>("Line.VertexShader", "Line.FragmentShader") },
		rectangleShader{ std::make_unique<Shader>("Rectangle.VertexShader", "Rectangle.FragmentShader") },
		back{ std::make_unique<Back>() },
		camera{ std::make_unique<Camera>() },
		cursor{ std::make_unique<Cursor>(*camera.get(), display, *cursorShader.get()) },
		grid{ std::make_unique<Grid>(*camera.get(), *lineShader.get()) },
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
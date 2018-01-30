#include "Elements.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements() 
		: back{ std::make_unique<Back>() },
		camera{ std::make_unique<Camera>() },
		imageShader{ std::make_unique<Shader>("Image.VertexShader", "Image.FragmentShader") },
		lineShader{ std::make_unique<Shader>("Line.VertexShader", "Line.FragmentShader") },
		notes{ std::make_unique<Notes>() },
		rectangleShader{ std::make_unique<Shader>("Rectangle.VertexShader", "Rectangle.FragmentShader") },
		sprites{ std::make_unique<Sprites>() } {
		// Grid needs to be separately initialized after lineShader is set
		grid = std::make_unique<Grid>(lineShader.get());
		all = { camera.get(), back.get(), sprites.get(), grid.get(), notes.get() };
	}
	void Elements::draw() {
		back->draw(*camera.get());
		sprites->draw(*camera.get());
		grid->draw(*camera.get());
		notes->draw(*camera.get());
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
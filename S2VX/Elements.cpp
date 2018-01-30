#include "Elements.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements() 
		: back{ std::make_unique<Back>() },
		camera{ std::make_unique<Camera>() },
		grid{ std::make_unique<Grid>(lineShader.get()) },
		notes{ std::make_unique<Notes>() },
		sprites{ std::make_unique<Sprites>() },
		all{ camera.get(), back.get(), sprites.get(), grid.get(), notes.get() } {}
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
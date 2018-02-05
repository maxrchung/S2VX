#include "Elements.hpp"
#include "Display.hpp"
#include <algorithm>
namespace S2VX {
	Elements::Elements(const Display& display) 
		: back{ Back() },
		camera{ Camera() },
		cursor{ Cursor(camera, display, cursorShader) },
		cursorShader{ R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Cursor.VertexShader)", R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Cursor.FragmentShader)" },
		grid{ Grid(camera, lineShader) },
		imageShader{ R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Image.VertexShader)", R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Image.FragmentShader)" },
		lineShader{ R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.VertexShader)", R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Line.FragmentShader)" },
		notes{ Notes() },
		rectangleShader{ R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Rectangle.VertexShader)", R"(C:\Users\Wax Chug da Gwad\Desktop\S2VX\S2VX\Rectangle.FragmentShader)" },
		sprites{ Sprites() },
		all{ &camera, &back, &sprites, &grid, &notes, &cursor } {}
	Elements::Elements(Elements&& other)
		: back{ std::move(other.back) },
		camera{ std::move(other.camera) },
		cursor{ std::move(other.cursor) },
		grid{ std::move(other.grid) },
		noteConfiguration{ other.noteConfiguration },
		notes{ std::move(other.notes) },
		sprites{ std::move(other.sprites) },
		textures{ std::move(other.textures) },
		all { &camera, &back, &sprites, &grid, &notes, &cursor } {}
	Elements& Elements::operator=(Elements&& other) {
		if (this != &other) {
			back = std::move(other.back);
			camera = std::move(other.camera);
			cursor = std::move(other.cursor);
			grid = std::move(other.grid);
			noteConfiguration = other.noteConfiguration;
			notes = std::move(other.notes);
			sprites = std::move(other.sprites);
			textures = std::move(other.textures);
			all = { &camera, &back, &sprites, &grid, &notes, &cursor };
		}
		return *this;
	}
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
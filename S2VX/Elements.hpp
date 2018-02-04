#pragma once
// Includes necessary for unique_ptr to access destructor(?)
#include "Back.hpp"
#include "Camera.hpp"
#include "Cursor.hpp"
#include "Grid.hpp"
#include "NoteConfiguration.hpp"
#include "Notes.hpp"
#include "Shader.hpp"
#include "Sprites.hpp"
#include "Texture.hpp"
#include <memory>
#include <unordered_map>
#include <vector>
namespace S2VX {
	class Command;
	class Display;
	class Element;
	class Note;
	class Sprite;
	// Structure for holding and handling all elements returned from Scripting evaluate()
	class Elements {
	public:
		Elements(const Display& display);
		Back* const getBack() { return back.get(); }
		Camera* const getCamera() { return camera.get(); }
		Cursor* const getCursor() { return cursor.get(); }
		Grid* const getGrid() { return grid.get(); }
		Notes* const getNotes() { return notes.get(); }
		NoteConfiguration& getNoteConfiguration() { return noteConfiguration; }
		Sprites* const getSprites() { return sprites.get(); }
		Shader* const getRectangleShader() { return rectangleShader.get(); }
		Shader* const getImageShader() { return imageShader.get(); }
		std::unordered_map<std::string, Texture>& getTextures() { return textures; }
		void draw();
		void update(const int time);
		// Sorts Commands/Elements into order by start time
		void sort();
	private:
		// Shaders need to be loaded before elements
		std::unique_ptr<Shader> cursorShader;
		std::unique_ptr<Shader> lineShader;
		std::unique_ptr<Shader> rectangleShader;
		std::unique_ptr<Shader> imageShader;
		// Camera needs to be before some elements
		std::unique_ptr<Camera> camera;
		// Tracks loaded textures
		std::unique_ptr<Back> back;
		std::unique_ptr<Cursor> cursor;
		std::unique_ptr<Grid> grid;
		std::unique_ptr<Notes> notes;
		std::unique_ptr<Sprites> sprites;
		// Holds everything
		std::vector<Element*> all;
		// Tracks current note config
		NoteConfiguration noteConfiguration;
		std::unordered_map<std::string, Texture> textures;
	};
}
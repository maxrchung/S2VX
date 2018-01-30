#pragma once
// Includes necessary for unique_ptr to access destructor(?)
#include "Back.hpp"
#include "Camera.hpp"
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
	class Element;
	class Note;
	class Sprite;
	// Structure for holding and handling all elements returned from Scripting evaluate()
	class Elements {
	public:
		Elements();
		Back* const getBack() { return back.get(); }
		Camera* const getCamera() { return camera.get(); }
		Grid* const getGrid() { return grid.get(); }
		Notes* const getNotes() { return notes.get(); }
		NoteConfiguration& getNoteConfiguration() { return noteConfiguration; }
		Sprites* const getSprites() { return sprites.get(); }
		Shader* const getRectangleShader() { return rectangleShader.get(); }
		Shader* const getImageShader() { return imageShader.get(); }
		std::unordered_map<std::string, std::unique_ptr<Texture>>& getTextures() { return textures; }
		void draw();
		void update(const int time);
		// Sorts Commands/Elements into order by start time
		void sort();
	private:
		std::unique_ptr<Back> back;
		std::unique_ptr<Camera> camera;
		std::unique_ptr<Grid> grid;
		std::unique_ptr<Notes> notes;
		std::unique_ptr<Sprites> sprites;
		// Holds everything
		std::vector<Element*> all;
		// Tracks current note config
		NoteConfiguration noteConfiguration;
		// Tracks loaded textures
		std::unique_ptr<Shader> lineShader;
		std::unique_ptr<Shader> rectangleShader;
		std::unique_ptr<Shader> imageShader;
		std::unordered_map<std::string, std::unique_ptr<Texture>> textures;
	};
}
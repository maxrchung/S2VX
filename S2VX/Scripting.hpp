#pragma once
#include "Command.hpp"
#include "Element.hpp"
#include "Elements.hpp"
#include "EasingType.hpp"
#include "Sprite.hpp"
#include "Texture.hpp"
#include <chaiscript/chaiscript.hpp>
#include <memory>
#include <set>
#include <unordered_map>
namespace S2VX {
	class Scripting {
	public:
		// Initializing scripting
		Scripting();
		void BackColor(int start, int end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA);
		void CameraMove(int start, int end, int easing, int startX, int startY, int endX, int endY);
		void CameraRotate(int start, int end, int easing, float startRotate, float endRotate);
		void CameraZoom(int start, int end, int easing, float startScale, float endScale);
		// Evaluates chaiscript
		Elements evaluate(const std::string& path);
		void GridFeather(int start, int end, int easing, float startFeather, float endFeather);
		void GridThickness(int start, int end, int easing, float startThickness, float endThickness);
		void NoteBind(int time, int x, int y);
		void SpriteBind(const std::string& path);
		void SpriteFade(int start, int end, int easing, float startFade, float endFade);
		void SpriteMoveX(int start, int end, int easing, int startX, int endX);
		void SpriteMoveY(int start, int end, int easing, int startY, int endY);
		void SpriteRotate(int start, int end, int easing, float startRotation, float endRotation);
		void SpriteScale(int start, int end, int easing, float startScaleX, float startScaleY, float endScaleX, float endScaleY);
	private:
		// Converts sortedCommands to vector form
		std::vector<Command*> sortedCommandsToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands);
		std::vector<Note*> sortedNotesToVector();
		std::vector<Sprite*> sortedSpritesToVector();
		void addSprite();
		void reset();
		chaiscript::ChaiScript chai;
		NoteConfiguration noteConfiguration;
		// Keeps track of the current Sprite's commands
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> currentSpriteCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedBackCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedCameraCommands;
		std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison> sortedGridCommands;
		std::multiset<std::unique_ptr<Note>, NoteUniquePointerComparison> sortedNotes;
		std::multiset<std::unique_ptr<Sprite>, SpriteUniquePointerComparison> sortedSprites;
		std::unique_ptr<Shader> imageShader = std::make_unique<Shader>("Image.VertexShader", "Image.FragmentShader");
		std::unique_ptr<Shader> rectangleShader = std::make_unique<Shader>("Rectangle.VertexShader", "Rectangle.FragmentShader");
		// Pointer because destructor cleans up OpenGL objects
		std::unordered_map<std::string, std::unique_ptr<Texture>> spriteTextures;
		// Keeps track of the current Sprite's Texture
		Texture* currentTexture;
	};
}
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
		void BackColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float startA, const float endR, const float endG, const float endB, const float endA);
		void CameraMove(const int start, const int end, const int easing, const int startX, const int startY, const int endX, const int endY);
		void CameraRotate(const int start, const int end, const int easing, const float startRotate, const float endRotate);
		void CameraZoom(const int start, const int end, const int easing, const float startScale, const float endScale);
		// Evaluates chaiscript
		Elements evaluate(const std::string& path);
		void GridFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather);
		void GridThickness(const int start, const int end, const int easing, const float startThickness, const float endThickness);
		void NoteBind(const int time, const int x, const int y);
		void SpriteBind(const std::string& path);
		void SpriteFade(const int start, const int end, const int easing, const float startFade, const float endFade);
		void SpriteMove(const int start, const int end, const int easing, const int startX, const int startY, const int endX, const int endY);
		void SpriteRotate(const int start, const int end, const int easing, const float startRotation, const float endRotation);
		void SpriteScale(const int start, const int end, const int easing, const float startScaleX, const float startScaleY, const float endScaleX, const float endScaleY);
	private:
		// Converts sortedCommands to vector form
		std::vector<Command*> sortedCommandsToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands);
		std::vector<Note*> sortedNotesToVector();
		std::vector<Sprite*> sortedSpritesToVector();
		// Adds current sprite to sprites multiset
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
		std::unique_ptr<Shader> lineShader = std::make_unique<Shader>("Line.VertexShader", "Line.FragmentShader");
		std::unique_ptr<Shader> rectangleShader = std::make_unique<Shader>("Rectangle.VertexShader", "Rectangle.FragmentShader");
		// Pointer because destructor cleans up OpenGL objects
		std::unordered_map<std::string, std::unique_ptr<Texture>> spriteTextures;
		// Keeps track of the current Sprite's Texture
		const Texture* currentTexture;
	};
}
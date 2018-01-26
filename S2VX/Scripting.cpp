#include "Scripting.hpp"
#include "BackColorCommand.hpp"
#include "CameraMoveCommand.hpp"
#include "CameraRotateCommand.hpp"
#include "CameraZoomCommand.hpp"
#include "GridColorCommand.hpp"
#include "GridFadeCommand.hpp"
#include "GridFeatherCommand.hpp"
#include "GridThicknessCommand.hpp"
#include "SpriteColorCommand.hpp"
#include "SpriteFadeCommand.hpp"
#include "SpriteMoveCommand.hpp"
#include "SpriteRotateCommand.hpp"
#include "SpriteScaleCommand.hpp"
#include "CameraCommand.hpp"
#include "Elements.hpp"
#include "GridCommand.hpp"
#include "ScriptError.hpp"
#include "SpriteCommand.hpp"
#include "Texture.hpp"
namespace S2VX {
	Scripting::Scripting() {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::BackColor, this), "BackColor");
		chai.add(chaiscript::fun(&Scripting::CameraMove, this), "CameraMove");
		chai.add(chaiscript::fun(&Scripting::CameraRotate, this), "CameraRotate");
		chai.add(chaiscript::fun(&Scripting::CameraZoom, this), "CameraZoom");
		chai.add(chaiscript::fun(&Scripting::GridColor, this), "GridColor");
		chai.add(chaiscript::fun(&Scripting::GridFade, this), "GridFade");
		chai.add(chaiscript::fun(&Scripting::GridFeather, this), "GridFeather");
		chai.add(chaiscript::fun(&Scripting::GridThickness, this), "GridThickness");;
		chai.add(chaiscript::fun(&Scripting::NoteApproach, this), "NoteApproach");
		chai.add(chaiscript::fun(&Scripting::NoteBind, this), "NoteBind");
		chai.add(chaiscript::fun(&Scripting::NoteColor, this), "NoteColor");
		chai.add(chaiscript::fun(&Scripting::NoteDistance, this), "NoteDistance");
		chai.add(chaiscript::fun(&Scripting::NoteFadeIn, this), "NoteFadeIn");
		chai.add(chaiscript::fun(&Scripting::NoteFadeOut, this), "NoteFadeOut");
		chai.add(chaiscript::fun(&Scripting::NoteFeather, this), "NoteFeather");
		chai.add(chaiscript::fun(&Scripting::NoteThickness, this), "NoteThickness");
		chai.add(chaiscript::fun(&Scripting::SpriteBind, this), "SpriteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteColor, this), "SpriteColor");
		chai.add(chaiscript::fun(&Scripting::SpriteFade, this), "SpriteFade");
		chai.add(chaiscript::fun(&Scripting::SpriteMove, this), "SpriteMove");
		chai.add(chaiscript::fun(&Scripting::SpriteRotate, this), "SpriteRotate");
		chai.add(chaiscript::fun(&Scripting::SpriteScale, this), "SpriteScale");
	}
	Elements Scripting::evaluate(const std::string& path) {
		reset();
		chai.use(path);
		// Handle last sprite
		addSprite();
		// I think it is okay to use raw pointer because the ownership should be handled by sortedCommands
		auto backCommands = sortedCommandsToVector(sortedBackCommands);
		auto cameraCommands = sortedCommandsToVector(sortedCameraCommands);
		auto gridCommands = sortedCommandsToVector(sortedGridCommands);
		auto notes = sortedNotesToVector();
		auto sprites = sortedSpritesToVector();
		auto elements = Elements(backCommands,
								 cameraCommands,
								 gridCommands, lineShader.get(),
								 notes,
								 sprites);
		return elements;
	}
	std::vector<Command*> Scripting::sortedCommandsToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands) {
		auto commands = std::vector<Command*>(sortedCommands.size());
		std::transform(sortedCommands.begin(), sortedCommands.end(), commands.begin(), [](auto& command) { return command.get(); });
		return commands;
	}
	std::vector<Note*> Scripting::sortedNotesToVector() {
		auto notes = std::vector<Note*>(sortedNotes.size());
		std::transform(sortedNotes.begin(), sortedNotes.end(), notes.begin(), [](auto& note) { return note.get(); });
		return notes;
	}
	std::vector<Sprite*> Scripting::sortedSpritesToVector() {
		auto sprites = std::vector<Sprite*>(sortedSprites.size());
		std::transform(sortedSprites.begin(), sortedSprites.end(), sprites.begin(), [](auto& sprite) { return sprite.get(); });
		return sprites;
	}
	void Scripting::addSprite() {
		// Throw exception if a sprite was made with no commands
		if (currentTexture) {
			if (currentSpriteCommands.empty()) {
				throw ScriptError("Sprite must have at least one command. Given: " + currentTexture->getPath());
			}
			// Only add sprite if texture has been set
			else {
				const auto spriteCommands = sortedCommandsToVector(currentSpriteCommands);
				sortedSprites.insert(std::make_unique<Sprite>(spriteCommands, currentTexture, imageShader.get()));
			}
		}
	}
	void Scripting::BackColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		sortedBackCommands.insert(std::make_unique<BackColorCommand>(start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::CameraMove(const int start, const int end, const int easing, const int startX, const int startY, const int endX, const int endY) {
		const auto convert = static_cast<EasingType>(easing);
		sortedCameraCommands.insert(std::make_unique<CameraMoveCommand>(start, end, convert, startX, startY, endX, endY));
	}
	void Scripting::CameraRotate(const int start, const int end, const int easing, const float startRotate, const float endRotate) {
		const auto convert = static_cast<EasingType>(easing);
		sortedCameraCommands.insert(std::make_unique<CameraRotateCommand>(start, end, convert, startRotate, endRotate));
	}
	void Scripting::CameraZoom(const int start, const int end, const int easing, const float startScale, const float endScale) {
		const auto convert = static_cast<EasingType>(easing);
		sortedCameraCommands.insert(std::make_unique<CameraZoomCommand>(start, end, convert, startScale, endScale));
	}
	void Scripting::GridColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		sortedGridCommands.insert(std::make_unique<GridColorCommand>(start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::GridFade(const int start, const int end, const int easing, const float startFade, const float endFade) {
		const auto convert = static_cast<EasingType>(easing);
		sortedGridCommands.insert(std::make_unique<GridFadeCommand>(start, end, convert, startFade, endFade));
	}
	void Scripting::GridFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather) {
		const auto convert = static_cast<EasingType>(easing);
		sortedGridCommands.insert(std::make_unique<GridFeatherCommand>(start, end, convert, startFeather, endFeather));
	}
	void Scripting::GridThickness(const int start, const int end, const int easing, const float startThickness, const float endThickness) {
		const auto convert = static_cast<EasingType>(easing);
		sortedGridCommands.insert(std::make_unique<GridThicknessCommand>(start, end, convert, startThickness, endThickness));
	}
	void Scripting::NoteApproach(const int approach) {
		noteConfiguration.setApproach(approach);
	}
	void Scripting::NoteBind(const int time, const int x, const int y) {
		noteConfiguration.setEnd(time);
		const auto position = glm::vec2{ x, y };
		noteConfiguration.setPosition(position);
		sortedNotes.insert(std::make_unique<Note>(noteConfiguration, rectangleShader.get()));
	}
	void Scripting::NoteColor(const int r, const int g, const int b) {
		const auto color = glm::vec3{ r, g, b };
		noteConfiguration.setColor(color);
	}
	void Scripting::NoteDistance(const float distance) {
		noteConfiguration.setDistance(distance);
	}
	void Scripting::NoteFadeIn(const int fadeIn) {
		noteConfiguration.setFadeIn(fadeIn);
	}
	void Scripting::NoteFadeOut(const int fadeOut) {
		noteConfiguration.setFadeOut(fadeOut);
	}
	void Scripting::NoteFeather(const float feather) {
		noteConfiguration.setFeather(feather);
	}
	void Scripting::NoteThickness(const float thickness) {
		noteConfiguration.setThickness(thickness);
	}
	void Scripting::reset() {
		currentSpriteCommands.clear();
		currentTexture = nullptr;
		sortedBackCommands.clear();
		sortedCameraCommands.clear();
		sortedGridCommands.clear();
		sortedNotes.clear();
		sortedSprites.clear();
		spriteTextures.clear();
	}
	void Scripting::SpriteBind(const std::string& path) {
		addSprite();
		// Otherwise start setting up Texture for next Sprite
		if (spriteTextures.find(path) == spriteTextures.end()) {
			spriteTextures[path] = std::make_unique<Texture>(path);
		}
		currentTexture = spriteTextures[path].get();
	}
	void Scripting::SpriteColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		currentSpriteCommands.insert(std::make_unique<SpriteColorCommand>(start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::SpriteFade(const int start, const int end, const int easing, const float startFade, const float endFade) {
		const auto convert = static_cast<EasingType>(easing);
		currentSpriteCommands.insert(std::make_unique<SpriteFadeCommand>(start, end, convert, startFade, endFade));
	}
	void Scripting::SpriteMove(const int start, const int end, const int easing, const int startX, const int startY, const int endX, const int endY) {
		const auto convert = static_cast<EasingType>(easing);
		currentSpriteCommands.insert(std::make_unique<SpriteMoveCommand>(start, end, convert, startX, startY, endX, endY));
	}
	void Scripting::SpriteRotate(const int start, const int end, const int easing, const float startRotation, const float endRotation) {
		const auto convert = static_cast<EasingType>(easing);
		currentSpriteCommands.insert(std::make_unique<SpriteRotateCommand>(start, end, convert, startRotation, endRotation));
	}
	void Scripting::SpriteScale(const int start, const int end, const int easing, const float startScaleX, const float startScaleY, const float endScaleX, const float endScaleY) {
		const auto convert = static_cast<EasingType>(easing);
		currentSpriteCommands.insert(std::make_unique<SpriteScaleCommand>(start, end, convert, startScaleX, startScaleY, endScaleX, endScaleY));
	}
}
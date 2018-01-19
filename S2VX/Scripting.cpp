#include "Scripting.hpp"
#include "BackCommands.hpp"
#include "CameraCommands.hpp"
#include "GridCommands.hpp"
#include "SpriteCommands.hpp"
namespace S2VX {
	Scripting::Scripting() {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::BackColor, this), "BackColor");
		chai.add(chaiscript::fun(&Scripting::CameraMove, this), "CameraMove");
		chai.add(chaiscript::fun(&Scripting::CameraRotate, this), "CameraRotate");
		chai.add(chaiscript::fun(&Scripting::CameraZoom, this), "CameraZoom");
		chai.add(chaiscript::fun(&Scripting::GridFeather, this), "GridFeather");
		chai.add(chaiscript::fun(&Scripting::GridThickness, this), "GridThickness");
		chai.add(chaiscript::fun(&Scripting::NoteBind, this), "NoteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteBind, this), "SpriteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteFade, this), "SpriteFade");
		chai.add(chaiscript::fun(&Scripting::SpriteMoveX, this), "SpriteMoveX");
		chai.add(chaiscript::fun(&Scripting::SpriteMoveY, this), "SpriteMoveY");
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
		auto elements = Elements(std::make_unique<Back>(backCommands),
								 std::make_unique<Camera>(cameraCommands),
								 std::make_unique<Grid>(gridCommands),
								 std::make_unique<Notes>(notes),
								 std::make_unique<Sprites>(sprites));
		return elements;
	}
	std::vector<Command*> Scripting::sortedCommandsToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands) {
		auto vector = std::vector<Command*>(sortedCommands.size());
		int i = 0;
		// This should be a little faster than just adding by push_back?
		for (auto& command : sortedCommands) {
			vector[i++] = command.get();
		}
		return vector;
	}
	std::vector<Note*> Scripting::sortedNotesToVector() {
		auto vector = std::vector<Note*>(sortedNotes.size());
		int i = 0;
		for (auto& command : sortedNotes) {
			vector[i++] = command.get();
		}
		return vector;
	}
	std::vector<Sprite*> Scripting::sortedSpritesToVector() {
		auto vector = std::vector<Sprite*>(sortedSprites.size());
		int i = 0;
		sortedSprites.begin();
		for (auto& sprite : sortedSprites) {
			vector[i++] = sprite.get();
		}
		return vector;
	}
	void Scripting::addSprite() {
		// Bind previous sprite if there are commands
		if (!currentSpriteCommands.empty()) {
			auto spriteCommands = sortedCommandsToVector(currentSpriteCommands);
			sortedSprites.insert(std::make_unique<Sprite>(spriteCommands, currentTexture, imageShader.get()));
		}
	}
	void Scripting::BackColor(int start, int end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<BackColorCommand>(start, end, convert, startR, startG, startB, startA, endR, endG, endB, endA);
		sortedBackCommands.insert(std::move(command));
	}
	void Scripting::CameraMove(int start, int end, int easing, int startX, int startY, int endX, int endY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraMoveCommand>(start, end, convert, startX, startY, endX, endY);
		sortedCameraCommands.insert(std::move(command));
	}
	void Scripting::CameraRotate(int start, int end, int easing, float startRotate, float endRotate) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraRotateCommand>(start, end, convert, startRotate, endRotate);
		sortedCameraCommands.insert(std::move(command));
	}
	void Scripting::CameraZoom(int start, int end, int easing, float startScale, float endScale) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraZoomCommand>(start, end, convert, startScale, endScale);
		sortedCameraCommands.insert(std::move(command));
	}
	void Scripting::GridFeather(int start, int end, int easing, float startFeather, float endFeather) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<GridFeatherCommand>(start, end, convert, startFeather, endFeather);
		sortedGridCommands.insert(std::move(command));
	}
	void Scripting::GridThickness(int start, int end, int easing, float startThickness, float endThickness) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<GridThicknessCommand>(start, end, convert, startThickness, endThickness);
		sortedGridCommands.insert(std::move(command));
	}
	void Scripting::NoteBind(int time, int x, int y) {
		noteConfiguration.setEnd(time);
		noteConfiguration.setPosition(glm::vec2(x, y));
		sortedNotes.insert(std::make_unique<Note>(noteConfiguration, rectangleShader.get()));
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
	void Scripting::SpriteFade(int start, int end, int easing, float startFade, float endFade) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<SpriteFadeCommand>(start, end, convert, startFade, endFade);
		currentSpriteCommands.insert(std::move(command));
	}
	void Scripting::SpriteMoveX(int start, int end, int easing, int startX, int endX) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveXCommand>(start, end, convert, startX, endX);
		currentSpriteCommands.insert(std::move(command));
	}
	void Scripting::SpriteMoveY(int start, int end, int easing, int startY, int endY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveXCommand>(start, end, convert, startY, endY);
		currentSpriteCommands.insert(std::move(command));
	}
	void Scripting::SpriteRotate(int start, int end, int easing, float startRotation, float endRotation) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<SpriteRotateCommand>(start, end, convert, startRotation, endRotation);
		currentSpriteCommands.insert(std::move(command));
	}
	void Scripting::SpriteScale(int start, int end, int easing, float startScaleX, float startScaleY, float endScaleX, float endScaleY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<SpriteScaleCommand>(start, end, convert, startScaleX, startScaleY, endScaleX, endScaleY);
		currentSpriteCommands.insert(std::move(command));
	}
}
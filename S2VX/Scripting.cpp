#include "Scripting.hpp"
#include "BackCommands.hpp"
#include "CameraCommands.hpp"
#include "GridCommands.hpp"
#include "NoteCommands.hpp"
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
	Elements Scripting::evaluate(const std::string& path) {
		reset();
		chai.use(path);
		// Record last sprite
		if (spriteID >= 0) {
			spriteStarts[spriteID] = spriteStart;
			spriteEnds[spriteID] = spriteEnd;
		}
		// Make create and destroy commands depending on recorded times
		for (auto& start : spriteStarts) {
			std::unique_ptr<Command> command = std::make_unique<SpriteCreateCommand>(start.second, start.first);
			sortedSpriteCommands.insert(std::move(command));
		}
		for (auto& end : spriteEnds) {
			std::unique_ptr<Command> command = std::make_unique<SpriteDeleteCommand>(end.second, end.first);
			sortedSpriteCommands.insert(std::move(command));
		}
		// I think it is okay to use raw pointer because the ownership should be handled by sortedCommands
		auto backCommands = sortedToVector(sortedBackCommands);
		auto cameraCommands = sortedToVector(sortedCameraCommands);
		auto gridCommands = sortedToVector(sortedGridCommands);
		auto noteCommands = sortedToVector(sortedNoteCommands);
		auto spriteCommands = sortedToVector(sortedSpriteCommands);
		auto elements = Elements(std::make_unique<Back>(backCommands),
								 std::make_unique<Camera>(cameraCommands),
								 std::make_unique<Grid>(gridCommands),
								 std::make_unique<Notes>(noteCommands),
								 std::make_unique<Sprites>(spriteCommands));
		return elements;
	}
	void Scripting::reset() {
		sortedBackCommands.clear();
		sortedCameraCommands.clear();
		sortedGridCommands.clear();
		sortedSpriteCommands.clear();
		sortedGridCommands.clear();
		spriteID = -1;
		resetSpriteTime();
	}
	void Scripting::resetSpriteTime() {
		spriteStart = std::numeric_limits<int>::max();
		std::unordered_map<int, int> spriteStarts;
		spriteEnd = 0;
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
	std::vector<Command*> Scripting::sortedToVector(const std::multiset<std::unique_ptr<Command>, CommandUniquePointerComparison>& sortedCommands) {
		auto vector = std::vector<Command*>(sortedCommands.size());
		int i = 0;
		// This should be a little faster than just adding by push_back?
		for (auto& command : sortedCommands) {
			vector[i++] = command.get();
		}
		return vector;
	}
	void Scripting::NoteBind(int time, int x, int y) {
		noteConfiguration.setEnd(time);
		noteConfiguration.setPosition(glm::vec2(x, y));
		std::unique_ptr<Command> command = std::make_unique<NoteBindCommand>(noteConfiguration.getStart(), noteConfiguration);
		sortedNoteCommands.insert(std::move(command));
	}
	void Scripting::SpriteBind(const std::string& path) {
		if (spriteID >= 0) {
			spriteStarts[spriteID] = spriteStart;
			spriteEnds[spriteID] = spriteEnd;
		}
		spriteID++;
		resetSpriteTime();
		std::unique_ptr<Command> command = std::make_unique<SpriteBindCommand>(spriteID, path);
		sortedSpriteCommands.insert(std::move(command));
	}
	void Scripting::SpriteFade(int start, int end, int easing, float startFade, float endFade) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteFadeCommand>(startTime, endTime, convert, spriteID, startFade, endFade);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
	void Scripting::SpriteMoveX(int start, int end, int easing, int startX, int endX) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveXCommand>(startTime, endTime, convert, spriteID, startX, endX);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
	void Scripting::SpriteMoveY(int start, int end, int easing, int startY, int endY) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveXCommand>(startTime, endTime, convert, spriteID, startY, endY);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
	void Scripting::SpriteRotate(int start, int end, int easing, float startRotation, float endRotation) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteRotateCommand>(startTime, endTime, convert, spriteID, startRotation, endRotation);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
	void Scripting::SpriteScale(int start, int end, int easing, float startScaleX, float startScaleY, float endScaleX, float endScaleY) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteScaleCommand>(startTime, endTime, convert, spriteID, startScaleX, startScaleY, endScaleX, endScaleY);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
}
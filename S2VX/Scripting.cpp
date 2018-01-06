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
		chai.add(chaiscript::fun(&Scripting::SpriteBind, this), "SpriteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteMove, this), "SpriteMove");
	}
	void Scripting::BackColor(int start, int end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<BackColorCommand>(start, end, convert, startR, startG, startB, startA, endR, endG, endB, endA);
		sortedBackCommands.insert(std::move(command));
	}
	void Scripting::CameraMove(int start, int end, int easing, float startX, float startY, float endX, float endY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraMoveCommand>(start, end, convert, startX, startY, endX, endY);
		sortedCameraCommands.insert(std::move(command));
	}
	void Scripting::CameraRotate(int start, int end, int easing, float startRoll, float endRoll) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraRotateCommand>(start, end, convert, startRoll, endRoll);
		sortedCameraCommands.insert(std::move(command));
	}
	void Scripting::CameraZoom(int start, int end, int easing, float startScale, float endScale) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraZoomCommand>(start, end, convert, startScale, endScale);
		sortedCameraCommands.insert(std::move(command));
	}
	Elements Scripting::evaluate(const std::string& path) {
		reset();
		try {
			chai.use(path);
		}
		catch (const chaiscript::exception::eval_error &e) {
			std::cout << "ChaiScript Error\n" << e.pretty_print() << '\n';
		}
		catch (const std::exception &e) {
			std::cout << e.what() << std::endl;
		}
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
		auto spriteCommands = sortedToVector(sortedSpriteCommands);
		auto elements = Elements(std::make_unique<Back>(backCommands),
								 std::make_unique<Camera>(cameraCommands),
								 std::make_unique<Grid>(gridCommands),
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
	void Scripting::SpriteMove(int start, int end, int easing, float startX, float startY, float endX, float endY) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = start;
		auto endTime = end;
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveCommand>(startTime, endTime, convert, spriteID, startX, startY, endX, endY);
		sortedSpriteCommands.insert(std::move(command));
		if (startTime < spriteStart) {
			spriteStart = startTime;
		}
		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}
}
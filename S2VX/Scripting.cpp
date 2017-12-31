#include "Scripting.hpp"

#include "CameraCommands.hpp"
#include "GridCommands.hpp"
#include "SpriteCommands.hpp"
#include "Grid.hpp"

namespace S2VX {
	Scripting::Scripting() {}

	void Scripting::init() {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::GridColorBack, this), "GridColorBack");
		chai.add(chaiscript::fun(&Scripting::CameraMove, this), "CameraMove");
		chai.add(chaiscript::fun(&Scripting::CameraRotate, this), "CameraRotate");
		chai.add(chaiscript::fun(&Scripting::CameraZoom, this), "CameraZoom");
		chai.add(chaiscript::fun(&Scripting::SpriteBind, this), "SpriteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteMove, this), "SpriteMove");
	}

	void Scripting::GridColorBack(const std::string& start, const std::string& end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<GridColorBackCommand>(Time(start), Time(end), convert, startR, startG, startB, startA, endR, endG, endB, endA);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraMoveCommand>(Time(start), Time(end), convert, startX, startY, endX, endY);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraRotate(const std::string& start, const std::string& end, int easing, float startRoll, float endRoll) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraRotateCommand>(Time(start), Time(end), convert, startRoll, endRoll);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraZoom(const std::string& start, const std::string& end, int easing, float startScale, float endScale) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CameraZoomCommand>(Time(start), Time(end), convert, startScale, endScale);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::SpriteBind(const std::string& path) {
		if (spriteID++ >= 0) {
			spriteStarts[spriteID] = spriteStart;
			spriteEnds[spriteID] = spriteEnd;
		}

		resetSpriteTime();

		std::unique_ptr<Command> command = std::make_unique<SpriteBindCommand>(spriteID, path);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::SpriteMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY) {
		auto convert = static_cast<EasingType>(easing);
		auto startTime = Time(start);
		auto endTime = Time(end);
		std::unique_ptr<Command> command = std::make_unique<SpriteMoveCommand>(startTime, endTime, convert, spriteID, startX, startY, endX, endY);
		sortedCommands.insert(std::move(command));

		if (startTime < spriteStart) {
			spriteStart = startTime;
		}

		if (endTime > spriteEnd) {
			spriteEnd = endTime;
		}
	}

	void Scripting::reset() {
		sortedCommands.clear();
		spriteID = -1;
		resetSpriteTime();
	}

	void Scripting::resetSpriteTime() {
		spriteStart = Time(std::numeric_limits<float>::max());
		std::unordered_map<int, Time> spriteStarts;
		spriteEnd = Time(0);
	}

	Elements Scripting::evaluate(const std::string& path) {
		reset();

		try {
			chai.use(path);
		}
		catch (const chaiscript::exception::eval_error &e) {
			std::cout << "Error\n" << e.pretty_print() << '\n';
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
			sortedCommands.insert(std::move(command));
		}
		for (auto& end : spriteEnds) {
			std::unique_ptr<Command> command = std::make_unique<SpriteDeleteCommand>(end.second, end.first);
			sortedCommands.insert(std::move(command));
		}

		// const iterator, can't move BiggerKappa                                   killme
		// I think it is okay to use raw pointer because the ownership should be handled by sortedCommands
		std::vector<Command*> cameraCommands;
		std::vector<Command*> gridCommands;
		std::vector<Command*> spriteCommands;
		for (auto& command : sortedCommands) {
			switch (command->elementType) {
				case ElementType::Camera:
					cameraCommands.push_back(command.get());
					break;
				case ElementType::Grid:
					gridCommands.push_back(command.get());
					break;
				case ElementType::Sprite:
					spriteCommands.push_back(command.get());
					break;
			}
		}

		Elements elements{ std::make_unique<Camera>(cameraCommands),
						   std::make_unique<Grid>(gridCommands),
						   std::make_unique<Sprites>(spriteCommands)};
		return elements;
	}
}
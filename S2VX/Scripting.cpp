#include "Scripting.hpp"

#include "CommandsCamera.hpp"
#include "CommandsGrid.hpp"
#include "Grid.hpp"

namespace S2VX {
	Scripting::Scripting() {}

	void Scripting::init() {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::GridColorBack, this), "GridColorBack");
		chai.add(chaiscript::fun(&Scripting::CameraMove, this), "CameraMove");
		chai.add(chaiscript::fun(&Scripting::CameraRotate, this), "CameraRotate");
		chai.add(chaiscript::fun(&Scripting::CameraZoom, this), "CameraZoom");
	}

	void Scripting::GridColorBack(const std::string& start, const std::string& end, int easing, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CommandGridColorBack>(Time(start), Time(end), convert, startR, startG, startB, startA, endR, endG, endB, endA);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraMove(const std::string& start, const std::string& end, int easing, float startX, float startY, float endX, float endY) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CommandCameraMove>(Time(start), Time(end), convert, startX, startY, endX, endY);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraRotate(const std::string& start, const std::string& end, int easing, float startRoll, float endRoll) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CommandCameraRotate>(Time(start), Time(end), convert, startRoll, endRoll);
		sortedCommands.insert(std::move(command));
	}

	void Scripting::CameraZoom(const std::string& start, const std::string& end, int easing, float startScale, float endScale) {
		auto convert = static_cast<EasingType>(easing);
		std::unique_ptr<Command> command = std::make_unique<CommandCameraZoom>(Time(start), Time(end), convert, startScale, endScale);
		sortedCommands.insert(std::move(command));
	}

	Elements Scripting::evaluate(const std::string& path) {
		sortedCommands.clear();
		try {
			chai.use(path);
		}
		catch (const chaiscript::exception::eval_error &e) {
			std::cout << "Error\n" << e.pretty_print() << '\n';
		}
		catch (const std::exception &e) {
			std::cout << e.what() << std::endl;
		}

		// const iterator, can't move BiggerKappa                                   killme
		// I think it is okay to use raw pointer because the ownership should be handled by sortedCommands
		std::vector<Command*> cameraCommands;
		std::vector<Command*> gridCommands;
		for (auto& command : sortedCommands) {
			switch (command->elementType) {
				case ElementType::Camera:
					cameraCommands.push_back(command.get());
					break;
				case ElementType::Grid:
					gridCommands.push_back(command.get());
					break;
			}
		}

		Elements elements{ std::make_unique<Camera>(cameraCommands),
						   std::make_unique<Grid>(gridCommands) };
		return elements;
	}
}
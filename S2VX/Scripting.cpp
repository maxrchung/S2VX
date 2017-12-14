#include "Scripting.hpp"

#include "CommandsGrid.hpp"
#include "Grid.hpp"

namespace S2VX {
	Scripting::Scripting() {}

	void Scripting::init() {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::GridColorBack, this), "GridColorBack");
	}

	void Scripting::GridColorBack(const std::string& start, const std::string& end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
		std::unique_ptr<Command> command = std::make_unique<CommandGridColorBack>(Time(start), Time(end), startR, startG, startB, startA, endR, endG, endB, endA);
		sortedCommands.insert(std::move(command));
	}

	std::vector<std::unique_ptr<Element>> Scripting::evaluate(const std::string& path) {
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

		std::vector<std::unique_ptr<Element>> elements;
		std::vector<Command*> gridCommands;
		for (auto& command : sortedCommands) {
			switch (command->elementType) {
				case ElementType::Grid:
					// const iterator, can't move BiggerKappa                                   killme
					// I think it is okay to use raw pointer because the ownership should be handled by sortedCommands
					gridCommands.push_back(command.get());
					break;
			}
		}

		std::unique_ptr<Element> grid = std::make_unique<Grid>(gridCommands);

		// Element ordering
		elements.push_back(std::move(grid));

		return elements;
	}
}
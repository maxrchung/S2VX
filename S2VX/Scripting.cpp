#include "Scripting.hpp"

#include "CommandsGrid.hpp"
#include "Grid.hpp"

Scripting::Scripting() {}

void Scripting::init() {
	instance = std::make_unique<Scripting>();
	chai.add(chaiscript::fun(&Scripting::GridColorBack, instance.get()), "GridColorBack");
}

void Scripting::GridColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
	std::unique_ptr<Command> command = std::make_unique<CommandGridColorBack>(start, end, startR, startG, startB, startA, endR, endG, endB, endA);
	sortedCommands.insert(std::move(command));
}

void Scripting::Test() {
	std::cout << "Test" << std::endl;
}

std::vector<std::unique_ptr<Element>> Scripting::evaluate(std::string path) {
	sortedCommands.clear();
	try {
		chai.eval_file(path);
	}
	catch (const chaiscript::exception::eval_error &e) {
		std::cout << "Error\n" << e.pretty_print() << '\n';
	}
	catch (const std::exception &e) {
		std::cout << e.what() << std::endl;
	}

	std::vector<std::unique_ptr<Element>> elements;

	std::vector<std::unique_ptr<Command>> gridCommands;
	for (auto& command : gridCommands) {
		switch (command->elementType) {
		case ElementType::Grid:
			// const iterator, can't move Big Kappa                                   killme
			gridCommands.push_back(std::move(command));
			break;
		}
	}

	std::unique_ptr<Element> grid = std::make_unique<Grid>(gridCommands);

	// Element ordering
	elements.push_back(std::move(grid));

	return elements;
}
#include "Scripting.hpp"

#include "CommandsGrid.hpp"
#include "Grid.hpp"

Scripting::Scripting() 
	: instance{ new Scripting } {
	chai.add(chaiscript::var(this), "S2VX");
}

void Scripting::Grid_ColorBack(int start, int end, float startR, float startG, float startB, float startA, float endR, float endG, float endB, float endA) {
	std::unique_ptr<Command> command = std::make_unique<CommandGrid_ColorBack>(start, end, startR, startG, startB, startA, endR, endG, endB, endA);
	sortedCommands.insert(std::move(command));
}

std::vector<std::unique_ptr<Element>> Scripting::evaluate(std::string path) {
	sortedCommands.clear();
	chai.use(path);

	std::vector<std::unique_ptr<Element>> elements;
	std::vector<std::unique_ptr<Command>> gridCommands;
	for (auto& command : sortedCommands) {
		switch (command->elementType) {
		case ElementType::Grid:
			gridCommands.push_back(command);
			break;
		}
	}

	std::unique_ptr<Element> grid = std::make_unique<Grid>(gridCommands);

	// Element ordering
	elements.push_back(std::move(grid));

	return elements;
}
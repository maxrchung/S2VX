#include "Scripting.hpp"

#include "Grid.hpp"

Scripting::Scripting() 
	: instance{ new Scripting } {
	chai.add(chaiscript::var(this), "S2VX");
}

void Scripting::Grid_ColorBack(int start, int end, float startR, float startG, float startB, float endR, float endG, float endB) {
	auto parameter = CommandParameter("", { std::to_string(startR), std::to_string(startG), std::to_string(startB), std::to_string(endR), std::to_string(endG), std::to_string(endB)}, {});
	auto command = std::make_unique<Command>(CommandType::Grid_ColorBack, start, end, parameter);
	sortedCommands.insert(std::move(command));
}

std::vector<std::unique_ptr<Element>> Scripting::evaluate(std::string path) {
	sortedCommands.clear();
	chai.use(path);

	std::vector<std::unique_ptr<Element>> elements;
	std::vector<std::unique_ptr<Command>> gridCommands;
	for (auto& command : sortedCommands) {

	}

	auto grid = std::make_unique<Element>(gridCommands);

	// Element ordering
	elements.push_back(grid);

	return elements;
}
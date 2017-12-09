#include "Element.hpp"

Element::Element(std::vector<std::unique_ptr<Command>> pCommands)
	: commands(std::move(pCommands)) {}
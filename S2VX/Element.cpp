#include "Element.hpp"
#include <algorithm>
#include <iostream>

Element::Element(const std::vector<Command*>& pCommands)
	: commands{ pCommands } {}

void Element::updateActives(const Time& time) {
	for (auto active = actives.begin(); active != actives.end(); ) {
		if (commands[*active]->end <= time) {
			active = actives.erase(active);
			std::cout << "Removed: " << " " << time.format << std::endl;
		}
		else {
			active++;
		}
	}

	while (next != commands.size() && commands[next]->start <= time) {
		actives.insert(next++);
	}
}
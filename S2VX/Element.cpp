#include "Element.hpp"
#include <algorithm>
#include <iostream>
namespace S2VX {
	Element::Element(const std::vector<Command*>& pCommands)
		: commands{ pCommands } {
	}
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
		while (nextCommand != commands.size() && commands[nextCommand]->start <= time) {
			actives.insert(nextCommand++);
		}
	}
}
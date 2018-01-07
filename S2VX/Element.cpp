#include "Element.hpp"
#include <algorithm>
#include <iostream>
#include <string>
namespace S2VX {
	Element::Element(const std::vector<Command*>& pCommands)
		: commands{ pCommands } {
	}
	void Element::updateActives(int time) {
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (commands[*active]->end <= time) {
				std::cout << "Removed " << std::to_string(static_cast<int>(commands[*active]->commandType)) << " at " << time << std::endl;
				active = actives.erase(active);
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
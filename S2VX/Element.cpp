#include "Element.hpp"
#include <algorithm>
#include <string>
namespace S2VX {
	Element::Element(const std::vector<Command*>& pCommands)
		: commands{ pCommands } {
	}
	void Element::updateActives(int time) {
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (commands[*active]->end <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
		while (nextActive != commands.size() && commands[nextActive]->start <= time) {
			actives.insert(nextActive++);
		}
	}
}
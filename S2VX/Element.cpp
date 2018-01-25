#include "Element.hpp"
#include "Command.hpp"
namespace S2VX {
	Element::Element(const std::vector<Command*>& pCommands)
		: commands{ pCommands } {
	}
	void Element::update(const int time) {
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (commands[*active]->getEnd() <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
		while (nextActive != commands.size() && commands[nextActive]->getStart() <= time) {
			actives.insert(nextActive++);
		}
		for (auto active : actives) {
			const auto command = commands[active];
			command->update(time);
		}
	}
}
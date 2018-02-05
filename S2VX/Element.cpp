#include "Element.hpp"
#include "Easing.hpp"
#include <algorithm>
namespace S2VX {
	const CommandUniquePointerComparison Element::commandComparison;
	Element::Element()
		: nextActive{ 0 } {}
	void Element::addCommand(std::unique_ptr<Command>&& command) {
		commands.push_back(std::move(command));
	}
	void Element::update(const int time) {
		// Add new active commands
		// Run active commands
		// Remove active commands - This is done last so command end will always trigger
		while (nextActive != commands.size() && commands[nextActive]->getStart() <= time) {
			actives.insert(nextActive++);
		}
		for (const auto active : actives) {
			const auto& command = commands[active];
			const auto interpolation = static_cast<float>(time - command->getStart()) / (command->getEnd() - command->getStart());
			const auto easing = Easing(command->getEasing(), interpolation);
			command->update(easing);
		}
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (commands[*active]->getEnd() <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
	}
	void Element::sort() {
		std::sort(commands.begin(), commands.end(), commandComparison);
	}
}
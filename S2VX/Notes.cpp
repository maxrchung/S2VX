#include "Notes.hpp"
namespace S2VX {
	void Notes::addNote(Note&& note) {
		notes.push_back(std::move(note));
	}
	void Notes::draw() {
		for (auto active : actives) {
			notes[active].draw();
		}
	}
	void Notes::update(const int time) {
		while (nextActive != notes.size() && notes[nextActive].getConfiguration().getStart() <= time) {
			actives.insert(nextActive++);
		}
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (notes[*active].getConfiguration().getEnd() <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
		for (const auto active : actives) {
			notes[active].update(time);
		}
	}
	void Notes::sort() {
		std::sort(notes.begin(), notes.end(), comparison);
	}
}
#include "Notes.hpp"
namespace S2VX {
	void Notes::addNote(std::unique_ptr<Note>&& note) {
		notes.push_back(std::move(note));
	}
	void Notes::draw(const Camera& camera) {
		for (auto active : actives) {
			notes[active]->draw(camera);
		}
	}
	void Notes::update(const int time) {
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (notes[*active]->getConfiguration().getEnd() <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
		while (nextActive != notes.size() && notes[nextActive]->getConfiguration().getStart() <= time) {
			actives.insert(nextActive++);
		}
		for (const auto active : actives) {
			notes[active]->update(time);
		}
	}
	void Notes::sort() {
		std::sort(notes.begin(), notes.end(), comparison);
	}
}
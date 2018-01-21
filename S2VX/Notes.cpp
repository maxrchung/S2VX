#include "Notes.hpp"
namespace S2VX {
	Notes::Notes(const std::vector<Note*>& pNotes)
		: notes{ pNotes } {}
	void Notes::draw(const Camera& camera) {
		for (auto active : actives) {
			notes[active]->draw(camera);
		}
	}
	void Notes::update(const int time) {
		for (const auto active : actives) {
			notes[active]->update(time);
		}
	}
	void Notes::updateActives(const int time) {
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
	}
}
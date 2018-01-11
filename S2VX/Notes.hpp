#pragma once
#include "Element.hpp"
#include "Note.hpp"
#include <set>
namespace S2VX {
	class Notes : public Element {
	public:
		Notes(const std::vector<Command*>& commands);
		void draw(const Camera& camera);
		void update(int time);
	private:
		// Originally I was going to use a CircularQueue, but it's possible that
		// notes may disappear out of order due to different configuration; as such,
		// I'm relenting to sets for now
		std::set<std::unique_ptr<Note>> activeNotes;
	};
}
#pragma once
#include "Element.hpp"
#include <set>
namespace S2VX {
	class Note;
	class Notes : public Element {
	public:
		explicit Notes() {};
		explicit Notes(const std::vector<Note*>& pNotes);
		void draw(const Camera& camera);
		void update(const int time);
		void updateActives(const int time);
	private:
		std::vector<Note*> notes;
	};
}
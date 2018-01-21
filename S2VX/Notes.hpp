#pragma once
#include "Element.hpp"
#include "Note.hpp"
#include <set>
namespace S2VX {
	class Notes : public Element {
	public:
		explicit Notes(const std::vector<Note*>& pNotes);
		void draw(const Camera& camera);
		void update(const int time);
		void updateActives(const int time);
	private:
		std::vector<Note*> notes;
	};
}
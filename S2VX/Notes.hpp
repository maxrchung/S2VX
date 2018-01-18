#pragma once
#include "Element.hpp"
#include "Note.hpp"
#include <set>
namespace S2VX {
	class Notes : public Element {
	public:
		Notes(const std::vector<Note*>& pNotes);
		void draw(const Camera& camera);
		void update(int time);
		void updateActives(int time);
	private:
		std::vector<Note*> notes;
	};
}
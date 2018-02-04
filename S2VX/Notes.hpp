#pragma once
#include "Element.hpp"
#include "Note.hpp"
#include "NoteComparison.hpp"
namespace S2VX {
	class Notes : public Element {
	public:
		Notes() {};
		void addNote(Note&& note);
		void draw();
		void update(const int time);
		void sort();
	private:
		NoteComparison comparison;
		std::vector<Note> notes;
	};
}
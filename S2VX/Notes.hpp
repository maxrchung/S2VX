#pragma once
#include "Element.hpp"
#include "Note.hpp"
#include "NoteUniquePointerComparison.hpp"
namespace S2VX {
	class Notes : public Element {
	public:
		Notes() {};
		void addNote(std::unique_ptr<Note>&& note);
		void draw();
		void update(const int time);
		void sort();
	private:
		NoteUniquePointerComparison comparison;
		std::vector<std::unique_ptr<Note>> notes;
	};
}
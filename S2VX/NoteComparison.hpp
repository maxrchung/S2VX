#pragma once
#include <memory>
namespace S2VX {
	class Note;
	struct NoteComparison {
		// Sort by start time
		bool operator() (const Note& lhs, const Note& rhs) const;
	};
}
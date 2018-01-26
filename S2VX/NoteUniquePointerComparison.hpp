#pragma once
#include <memory>
namespace S2VX {
	class Note;
	struct NoteUniquePointerComparison {
		// Sort by start time
		bool operator() (const std::unique_ptr<Note>& lhs, const std::unique_ptr<Note>& rhs) const;
	};
}
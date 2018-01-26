#include "NoteUniquePointerComparison.hpp"
#include "Note.hpp"
namespace S2VX {
	bool NoteUniquePointerComparison::operator() (const std::unique_ptr<Note>& lhs, const std::unique_ptr<Note>& rhs) const {
		return lhs->getConfiguration().getStart() < rhs->getConfiguration().getStart();
	}
}
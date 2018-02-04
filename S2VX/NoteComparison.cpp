#include "NoteComparison.hpp"
#include "Note.hpp"
namespace S2VX {
	bool NoteComparison::operator() (const Note& lhs, const Note& rhs) const {
		return lhs.getConfiguration().getStart() < rhs.getConfiguration().getStart();
	}
}
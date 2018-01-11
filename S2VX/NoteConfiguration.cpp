#include "NoteConfiguration.hpp"
namespace S2VX {
	void NoteConfiguration::setEnd(int pEnd) {
		end = pEnd + fadeOut;
		start = pEnd - approach - start;
	}
}
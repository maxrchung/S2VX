#include "NoteConfiguration.hpp"
namespace S2VX {
	void NoteConfiguration::setEnd(const int pEnd) {
		end = pEnd + fadeOut;
		start = pEnd - approach - fadeIn;
	}
}
#include "CommandUniquePointerComparison.hpp"
#include "Command.hpp"
namespace S2VX {
	bool CommandUniquePointerComparison::operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) const {
		return lhs->getStart() < rhs->getStart();
	}
}
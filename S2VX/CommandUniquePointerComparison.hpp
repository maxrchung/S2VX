#pragma once
#include <memory>
namespace S2VX {
	class Command;
	struct CommandUniquePointerComparison {
		bool operator() (const std::unique_ptr<Command>& lhs, const std::unique_ptr<Command>& rhs) const;
	};
}
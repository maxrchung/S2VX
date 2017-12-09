#include "Command.hpp"

CommandParameter::CommandParameter(std::string pValue, std::vector<std::string> pValues, std::vector<CommandParameter> pChildren)
	: value(pValue), values(pValues), children(pChildren) {}

Command::Command(CommandType pType, int pStart, int pEnd, CommandParameter pParameter)
	: type(pType), start(pStart), end(pEnd), parameter(pParameter) {}
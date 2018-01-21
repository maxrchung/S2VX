#pragma once
#include <exception>
#include <string>
namespace S2VX {
	struct ScriptError : std::exception {
		explicit ScriptError(const std::string& message)
			: std::exception{ message.c_str() } {}
	};
}
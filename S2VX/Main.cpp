#include "S2VX.hpp"
#include "ScriptError.hpp"
#include <chaiscript/chaiscript.hpp>
#include <iostream>
std::string addWithNewLine(const char* what) {
	std::string message(what);
	if (message.size() > 0 && message[message.size() - 1] == '\n') {
		return message;
	}
	else {
		return message + "\n";
	}
}
int main(int argc, char** argv) {
	try {
		if (argc != 2) {
			throw S2VX::ScriptError("Invalid number of arguments. Expected 1 argument for script path.");
		}
		std::string script{ argv[1] };
		S2VX::S2VX S2VX{ script };
		S2VX.run();
	}
	catch (const chaiscript::exception::eval_error &e) {
		std::cout << "ChaiScript Exception" << std::endl << addWithNewLine(e.pretty_print().c_str());
	}
	catch (const S2VX::ScriptError &e) {
		std::cout << "Script Error" << std::endl << addWithNewLine(e.what());
	}
	catch (const std::exception &e) {
		std::cout << "General Exception" << std::endl << addWithNewLine(e.what());
	}
	std::cin.get();
}
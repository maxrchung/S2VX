#include <chaiscript/chaiscript.hpp>
#include "S2VX.hpp"
#include "ScriptError.hpp"
#include <iostream>
std::string addNewLine(const char* what) {
	std::string message(what);
	if (message.size() > 0 && message[message.size() - 1] == '\n') {
		return "";
	}
	else {
		return "\n";
	}
}
int main(int argc, char** argv) {
	try {
		if (argc != 2) {
			throw S2VX::ScriptError("Invalid number of arguments. Expected 1 argument for script path to the program.");
		}
		std::string script{ argv[1] };
		S2VX::S2VX S2VX{ script };
		S2VX.run();
	}
	catch (const chaiscript::exception::eval_error &e) {
		std::cout << "ChaiScript Exception" << std::endl << e.pretty_print() << addNewLine(e.pretty_print().c_str());
	}
	catch (const S2VX::ScriptError &e) {

		std::cout << "Script Error" << std::endl << e.what() << addNewLine(e.what());
	}
	catch (const std::exception &e) {
		std::cout << "General Exception" << std::endl << e.what() << addNewLine(e.what());
	}
	std::cin.get();
}
#include "S2VX.hpp"
#include <iostream>
int main(int argc, char** argv) {
	if (argc != 2) {
		std::cout << "Invalid number of arguments." << std::endl;
	}
	std::string script{ argv[1] };
	S2VX::S2VX S2VX{ script };
	S2VX.run();
}
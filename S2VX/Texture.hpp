#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Default texture returns as generic block
		Texture() {};
		Texture(const std::string& path);
	};
}

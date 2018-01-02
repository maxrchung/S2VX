#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Default texture returns as generic block
		Texture() {};
		// Cleanup imageTexture
		~Texture();
		Texture(const std::string& path);
	private:
		unsigned int imageTexture;
	};
}
#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Needed for Texture map
		Texture() {};
		// Cleanup imageTexture
		explicit Texture(const std::string& path);
		~Texture();
		Texture(Texture&& other);
		Texture& operator=(Texture &&other);
		unsigned int getImageTexture() const { return imageTexture; }
	private:
		Texture(const Texture&) = delete;
		Texture& operator=(const Texture&) = delete;
		static const unsigned char* blankData;
		unsigned int imageTexture;
	};
}
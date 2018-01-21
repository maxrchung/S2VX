#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Cleanup imageTexture
		explicit Texture(const std::string& path);
		~Texture();
		unsigned int getImageTexture() const { return imageTexture; }
	private:
		unsigned int imageTexture;
		// I copied this manually after loading blank.png :blobsweats:
		static const unsigned char* blankData;
	};
}
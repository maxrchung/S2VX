#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Cleanup imageTexture
		explicit Texture(const std::string& pPath);
		~Texture();
		const std::string& getPath() const { return path; }
		unsigned int getImageTexture() const { return imageTexture; }
	private:
		// I copied this manually after loading blank.png :blobsweats:
		static const unsigned char* blankData;
		std::string path;
		unsigned int imageTexture;
	};
}
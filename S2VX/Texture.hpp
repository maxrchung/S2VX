#pragma once
#include <string>
namespace S2VX {
	class Texture {
	public:
		// Needed for Texture map
		Texture() {};
		// Cleanup imageTexture
		explicit Texture(const std::string& pPath);
		~Texture();
		Texture(Texture&& other);
		Texture& operator=(Texture &&other);
		const std::string& getPath() const { return path; }
		unsigned int getImageTexture() const { return imageTexture; }
	private:
		std::string path;
		Texture(const Texture&) = delete;
		Texture& operator=(const Texture&) = delete;
		static const unsigned char* blankData;
		unsigned int imageTexture;
	};
}
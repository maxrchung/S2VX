#include "Texture.hpp"
#include <glad/glad.h>
#include <iostream>
#include <stb_image.h>
namespace S2VX {
	Texture::Texture(const std::string& path) {
		glGenTextures(1, &imageTexture);
		glBindTexture(GL_TEXTURE_2D, imageTexture);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		int width;
		int height;
		int channels;
		stbi_set_flip_vertically_on_load(true);
		auto data = stbi_load(path.c_str(), &width, &height, &channels, 0);
		if (data) {
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
			glGenerateMipmap(GL_TEXTURE_2D);
		}
		else {
			std::cout << "Unable to load texture: " + path << std::endl;
		}
		stbi_image_free(data);
	}
	Texture::~Texture() {
		glDeleteTextures(1, &imageTexture);
	}
}
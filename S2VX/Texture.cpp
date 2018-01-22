#include "Texture.hpp"
#include <glad/glad.h>
#include <stb_image.h>
#include "ScriptError.hpp"
namespace S2VX {
	const unsigned char* Texture::blankData = reinterpret_cast<unsigned char*>("ÿÿÿýýýýÝ$’‹\\Ý\x1f");
	Texture::Texture(const std::string& pPath)
		: path{ pPath } {
		glGenTextures(1, &imageTexture);
		glBindTexture(GL_TEXTURE_2D, imageTexture);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		if (path == "") {
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, 1, 1, 0, GL_RGB, GL_UNSIGNED_BYTE, blankData);
			glGenerateMipmap(GL_TEXTURE_2D);
		}
		else {
			int width;
			int height;
			int channels;
			stbi_set_flip_vertically_on_load(true);
			const auto data = stbi_load(path.c_str(), &width, &height, &channels, 0);
			if (data) {
				if (channels == 4) {
					glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
				}
				else {
					glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
				}
				glGenerateMipmap(GL_TEXTURE_2D);
			}
			else {
				throw ScriptError("Unable to load texture from given path. Given: " + path);
			}
			stbi_image_free(data);
		}
	}
	Texture::~Texture() {
		glDeleteTextures(1, &imageTexture);
	}
}
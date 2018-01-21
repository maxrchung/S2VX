#pragma once
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <string>
namespace S2VX {
	class Shader {
	public:
		Shader();
		explicit Shader(const std::string& vertexPath, const std::string& fragmentPath, const std::string& geometryPath = "");
		~Shader();
		void setBool(const std::string &name, const bool value) const;
		void setInt(const std::string &name, const int value) const;
		void setFloat(const std::string &name, const float value) const;
		void setVec2(const std::string &name, const glm::vec2 &value) const;
		void setVec2(const std::string &name, const float x, const float y) const;
		void setVec3(const std::string &name, const glm::vec3 &value) const;
		void setVec3(const std::string &name, const float x, const float y, const float z) const;
		void setVec4(const std::string &name, const glm::vec4 &value) const;
		void setVec4(const std::string &name, const float x, const float y, const float z, const float w) const;
		void setMat2(const std::string &name, const glm::mat2 &mat) const;
		void setMat3(const std::string &name, const glm::mat3 &mat) const;
		void setMat4(const std::string &name, const glm::mat4 &mat) const;
		// Use shader
		void use();
	private:
		// Checks for compilation/linker errors
		void checkCompileErrors(const GLuint shader, const std::string& type);
		unsigned int program;
	};
}

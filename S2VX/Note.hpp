#pragma once
#include "NoteConfiguration.hpp"
#include "RectanglePoints.hpp"
#include <array>
#include <memory>
namespace S2VX {
	class Camera;
	class RectanglePoints;
	class Shader;
	class Note {
	public:
		explicit Note(Camera& pCamera, const NoteConfiguration& pConfiguration, Shader& pShader);
		~Note();
		const NoteConfiguration& getConfiguration() { return configuration; }
		void draw();
		void update(const int time);
	private:
		Camera& camera;
		const NoteConfiguration configuration;
		const std::array<RectanglePoints, 4> lines;
		float scale;
		float fade;
		Shader& shader;
		unsigned int vertexArray;
		unsigned int vertexBuffer;
	};
}
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
		Note(Note&& other);
		Note& operator=(Note&& other);
		const NoteConfiguration& getConfiguration() const { return configuration; }
		void draw();
		void update(const int time);
	private:
		Note(const Note&) = delete;
		Note& operator=(const Note&) = delete;
		Camera& camera;
		const NoteConfiguration configuration;
		float fade;
		float scale;
		Shader& shader;
		static const std::array<RectanglePoints, 4> lines;
		unsigned int vertexArray;
		unsigned int vertexBuffer;
	};
}
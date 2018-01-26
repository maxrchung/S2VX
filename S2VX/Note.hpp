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
		explicit Note(const NoteConfiguration& pConfiguration, Shader* const pSquareShader);
		~Note();
		const NoteConfiguration& getConfiguration() { return configuration; }
		void draw(const Camera& camera);
		void update(const int time);
	private:
		const NoteConfiguration configuration;
		float scale;
		float fade;
		Shader* const squareShader;
		std::array<RectanglePoints, 4> lines;
		unsigned int squareVertexArray;
		unsigned int squareVertexBuffer;
	};
}
#pragma once
#include "Camera.hpp"
#include "NoteConfiguration.hpp"
#include "Shader.hpp"
namespace S2VX {
	class Note {
	public:
		Note(const NoteConfiguration& pConfiguration);
		~Note();
		void draw(const Camera& camera);
		void update(int time);
	private:
		NoteConfiguration configuration;
		std::unique_ptr<Shader> linesShader = std::make_unique<Shader>("Square.VertexShader", "Line.FragmentShader");
		unsigned int linesVertexArray;
		unsigned int linesVertexBuffer;
		static constexpr unsigned int linePointsLength = 96;
		static constexpr float linePoints[linePointsLength] = {
			// Position			// Normal
			 0.5f,	 0.5f,		 1.0f,	 0.0f,	// Right triangle top-right
			 0.5f,	-0.5f,		 1.0f,	 0.0f,	// Right triangle top-right
			 0.5f,	 0.5f,		-1.0f,	 0.0f,	// Right triangle top-right

			 0.5f,	-0.5f,		 1.0f,	 0.0f,	// Right triangle bottom-left
			 0.5f,	-0.5f,		-1.0f,	 0.0f,	// Right triangle bottom-left
			 0.5f,	 0.5f,		-1.0f,	 0.0f,	// Right triangle bottom-left

			 0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle top-right
			 0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle top-right
			-0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle top-right

			 0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle bottom-left
			-0.5f,	-0.5f,		 0.0f,	-1.0f,	// Bottom triangle bottom-left
			-0.5f,	-0.5f,		 0.0f,	 1.0f,	// Bottom triangle bottom-left

			-0.5f,	 0.5f,		 1.0f,	 0.0f,	// Left triangle top-right
			-0.5f,	-0.5f,		 1.0f,	 0.0f,	// Left triangle top-right
			-0.5f,	 0.5f,		-1.0f,	 0.0f,	// Left triangle top-right

			-0.5f,	-0.5f,		 1.0f,	 0.0f,	// Left triangle bottom-left
			-0.5f,	-0.5f,		-1.0f,	 0.0f,	// Left triangle bottom-left
			-0.5f,	 0.5f,		-1.0f,	 0.0f,	// Left triangle bottom-left

			 0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle top-right
			 0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle top-right
			-0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle top-right

			 0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle bottom-left
			-0.5f,	 0.5f,		 0.0f,	-1.0f,	// Top triangle bottom-left
			-0.5f,	 0.5f,		 0.0f,	 1.0f,	// Top triangle bottom-left;
		};
		// Interpolated distance
		float activeScale;
	};
}
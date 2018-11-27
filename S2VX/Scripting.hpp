#pragma once
// Includes are necessary for unique_ptr to use destructor(?)
#include "Elements.hpp"
#include <chaiscript/chaiscript.hpp>
namespace S2VX {
	// This class is responsible for hooking up with chai and creating their corresponding Commands and Elements
	class Scripting {
	public:
		// Initializing scripting
		Scripting(const Display& pDisplay);
		// Evaluates chaiscript
		Elements& evaluate(const std::string& path);
	private:
		void BackColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void CameraMove(const int start, const int end, const int easing, const float startX, const float startY, const float endX, const float endY);
		void CameraRotate(const int start, const int end, const int easing, const float startRotate, const float endRotate);
		void CameraZoom(const int start, const int end, const int easing, const float startScale, const float endScale);
		void CursorColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void CursorFade(const int start, const int end, const int easing, const float startFade, const float endFade);
		void CursorFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather);
		void CursorScale(const int start, const int end, int easing, const float startScale, const float endScale);
		void GridColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void GridFade(const int start, const int end, const int easing, const float startFade, const float endFade);
		void GridFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather);
		void GridThickness(const int start, const int end, const int easing, const float startThickness, const float endThickness);
		void NoteApproach(const int approach);
		void NoteBind(const int time, const int x, const int y);
		void NoteColor(const int r, const int g, const int b);
		void NoteDistance(const float distance);
		void NoteFadeIn(const int fadeIn);
		void NoteFadeOut(const int fadeOut);
		void NoteFeather(const float feather);
		void NoteThickness(const float thickness);
		void SpriteBind(const std::string& path);
		void SpriteColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB);
		void SpriteFade(const int start, const int end, const int easing, const float startFade, const float endFade);
		void SpriteMove(const int start, const int end, const int easing, const float startX, const float startY, const float endX, const float endY);
		void SpriteRotate(const int start, const int end, const int easing, const float startRotation, const float endRotation);
		void SpriteScale(const int start, const int end, const int easing, const float startScaleX, const float startScaleY, const float endScaleX, const float endScaleY);
		const Display& display;
		chaiscript::ChaiScript chai;
		Elements elements;
	};
}
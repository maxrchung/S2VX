#include "Scripting.hpp"
#include "BackColorCommand.hpp"
#include "CameraMoveCommand.hpp"
#include "CameraRotateCommand.hpp"
#include "CameraZoomCommand.hpp"
#include "CursorColorCommand.hpp"
#include "CursorFadeCommand.hpp"
#include "CursorFeatherCommand.hpp"
#include "CursorScaleCommand.hpp"
#include "Elements.hpp"
#include "GridColorCommand.hpp"
#include "GridFadeCommand.hpp"
#include "GridFeatherCommand.hpp"
#include "GridThicknessCommand.hpp"
#include "ScriptError.hpp"
#include "Sprite.hpp"
#include "SpriteColorCommand.hpp"
#include "SpriteFadeCommand.hpp"
#include "SpriteMoveCommand.hpp"
#include "SpriteRotateCommand.hpp"
#include "SpriteScaleCommand.hpp"
#include "Texture.hpp"
namespace S2VX {
	Scripting::Scripting(const Display& pDisplay)
		: display{ pDisplay }, elements{ display } {
		chai.add(chaiscript::var(this), "S2VX");
		chai.add(chaiscript::fun(&Scripting::BackColor, this), "BackColor");
		chai.add(chaiscript::fun(&Scripting::CameraMove, this), "CameraMove");
		chai.add(chaiscript::fun(&Scripting::CameraRotate, this), "CameraRotate");
		chai.add(chaiscript::fun(&Scripting::CameraZoom, this), "CameraZoom");
		chai.add(chaiscript::fun(&Scripting::CursorColor, this), "CursorColor");
		chai.add(chaiscript::fun(&Scripting::CursorFade, this), "CursorFade");
		chai.add(chaiscript::fun(&Scripting::CursorFeather, this), "CursorFeather");
		chai.add(chaiscript::fun(&Scripting::CursorScale, this), "CursorScale");
		chai.add(chaiscript::fun(&Scripting::GridColor, this), "GridColor");
		chai.add(chaiscript::fun(&Scripting::GridFade, this), "GridFade");
		chai.add(chaiscript::fun(&Scripting::GridFeather, this), "GridFeather");
		chai.add(chaiscript::fun(&Scripting::GridThickness, this), "GridThickness");;
		chai.add(chaiscript::fun(&Scripting::NoteApproach, this), "NoteApproach");
		chai.add(chaiscript::fun(&Scripting::NoteBind, this), "NoteBind");
		chai.add(chaiscript::fun(&Scripting::NoteColor, this), "NoteColor");
		chai.add(chaiscript::fun(&Scripting::NoteDistance, this), "NoteDistance");
		chai.add(chaiscript::fun(&Scripting::NoteFadeIn, this), "NoteFadeIn");
		chai.add(chaiscript::fun(&Scripting::NoteFadeOut, this), "NoteFadeOut");
		chai.add(chaiscript::fun(&Scripting::NoteFeather, this), "NoteFeather");
		chai.add(chaiscript::fun(&Scripting::NoteThickness, this), "NoteThickness");
		chai.add(chaiscript::fun(&Scripting::SpriteBind, this), "SpriteBind");
		chai.add(chaiscript::fun(&Scripting::SpriteColor, this), "SpriteColor");
		chai.add(chaiscript::fun(&Scripting::SpriteFade, this), "SpriteFade");
		chai.add(chaiscript::fun(&Scripting::SpriteMove, this), "SpriteMove");
		chai.add(chaiscript::fun(&Scripting::SpriteRotate, this), "SpriteRotate");
		chai.add(chaiscript::fun(&Scripting::SpriteScale, this), "SpriteScale");
	}
	Elements& Scripting::evaluate(const std::string& path) {
		// Create and manipulate a new Elements object
		// Choosing to use unique pointer so Shaders aren't destroyed during copy
		elements = Elements(display);
		chai.use(path);
		elements.sort();
		return elements;
	}
	void Scripting::BackColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getBack().addCommand(std::make_unique<BackColorCommand>(elements.getBack(), start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::CameraMove(const int start, const int end, const int easing, const float startX, const float startY, const float endX, const float endY) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCamera().addCommand(std::make_unique<CameraMoveCommand>(elements.getCamera(), start, end, convert, startX, startY, endX, endY));
	}
	void Scripting::CameraRotate(const int start, const int end, const int easing, const float startRotate, const float endRotate) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCamera().addCommand(std::make_unique<CameraRotateCommand>(elements.getCamera(), start, end, convert, startRotate, endRotate));
	}
	void Scripting::CameraZoom(const int start, const int end, const int easing, const float startScale, const float endScale) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCamera().addCommand(std::make_unique<CameraZoomCommand>(elements.getCamera(), start, end, convert, startScale, endScale));
	}
	void Scripting::CursorColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCursor().addCommand(std::make_unique<CursorColorCommand>(elements.getCursor(), start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::CursorFade(const int start, const int end, const int easing, const float startFade, const float endFade) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCursor().addCommand(std::make_unique<CursorFadeCommand>(elements.getCursor(), start, end, convert, startFade, endFade));
	}
	void Scripting::CursorFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCursor().addCommand(std::make_unique<CursorFeatherCommand>(elements.getCursor(), start, end, convert, startFeather, endFeather));
	}
	void Scripting::CursorScale(const int start, const int end, int easing, const float startScale, const float endScale) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getCursor().addCommand(std::make_unique<CursorScaleCommand>(elements.getCursor(), start, end, convert, startScale, endScale));
	}
	void Scripting::GridColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getGrid().addCommand(std::make_unique<GridColorCommand>(elements.getGrid(), start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::GridFade(const int start, const int end, const int easing, const float startFade, const float endFade) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getGrid().addCommand(std::make_unique<GridFadeCommand>(elements.getGrid(), start, end, convert, startFade, endFade));
	}
	void Scripting::GridFeather(const int start, const int end, const int easing, const float startFeather, const float endFeather) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getGrid().addCommand(std::make_unique<GridFeatherCommand>(elements.getGrid(), start, end, convert, startFeather, endFeather));
	}
	void Scripting::GridThickness(const int start, const int end, const int easing, const float startThickness, const float endThickness) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getGrid().addCommand(std::make_unique<GridThicknessCommand>(elements.getGrid(), start, end, convert, startThickness, endThickness));
	}
	void Scripting::NoteApproach(const int approach) {
		elements.getNoteConfiguration().setApproach(approach);
	}
	void Scripting::NoteBind(const int time, const int x, const int y) {
		auto noteConfiguration = elements.getNoteConfiguration();
		noteConfiguration.setEnd(time);
		const auto position = glm::vec2{ x, y };
		noteConfiguration.setPosition(position);
		elements.getNotes().addNote(Note(elements.getCamera(), noteConfiguration, elements.getRectangleShader()));
	}
	void Scripting::NoteColor(const int r, const int g, const int b) {
		const auto color = glm::vec3{ r, g, b };
		elements.getNoteConfiguration().setColor(color);
	}
	void Scripting::NoteDistance(const float distance) {
		elements.getNoteConfiguration().setDistance(distance);
	}
	void Scripting::NoteFadeIn(const int fadeIn) {
		elements.getNoteConfiguration().setFadeIn(fadeIn);
	}
	void Scripting::NoteFadeOut(const int fadeOut) {
		elements.getNoteConfiguration().setFadeOut(fadeOut);
	}
	void Scripting::NoteFeather(const float feather) {
		elements.getNoteConfiguration().setFeather(feather);
	}
	void Scripting::NoteThickness(const float thickness) {
		elements.getNoteConfiguration().setThickness(thickness);
	}
	void Scripting::SpriteBind(const std::string& path) {
		auto& textures = elements.getTextures();
		// Add texture if it is not in map
		if (textures.find(path) == textures.end()) {
			textures[path] = Texture(path);
		}
		elements.getSprites().addSprite(std::make_unique<Sprite>(elements.getCamera(), textures[path], elements.getImageShader()));
	}
	void Scripting::SpriteColor(const int start, const int end, const int easing, const float startR, const float startG, const float startB, const float endR, const float endG, const float endB) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getSprites().getLastSprite()->addCommand(std::make_unique<SpriteColorCommand>(elements.getSprites().getLastSprite(), start, end, convert, startR, startG, startB, endR, endG, endB));
	}
	void Scripting::SpriteFade(const int start, const int end, const int easing, const float startFade, const float endFade) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getSprites().getLastSprite()->addCommand(std::make_unique<SpriteFadeCommand>(elements.getSprites().getLastSprite(), start, end, convert, startFade, endFade));
	}
	void Scripting::SpriteMove(const int start, const int end, const int easing, const float startX, const float startY, const float endX, const float endY) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getSprites().getLastSprite()->addCommand(std::make_unique<SpriteMoveCommand>(elements.getSprites().getLastSprite(), start, end, convert, startX, startY, endX, endY));
	}
	void Scripting::SpriteRotate(const int start, const int end, const int easing, const float startRotation, const float endRotation) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getSprites().getLastSprite()->addCommand(std::make_unique<SpriteRotateCommand>(elements.getSprites().getLastSprite(), start, end, convert, startRotation, endRotation));
	}
	void Scripting::SpriteScale(const int start, const int end, const int easing, const float startScaleX, const float startScaleY, const float endScaleX, const float endScaleY) {
		const auto convert = static_cast<EasingType>(easing);
		elements.getSprites().getLastSprite()->addCommand(std::make_unique<SpriteScaleCommand>(elements.getSprites().getLastSprite(), start, end, convert, startScaleX, startScaleY, endScaleX, endScaleY));
	}
}
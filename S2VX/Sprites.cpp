#include "Sprites.hpp"
#include "Sprite.hpp"
namespace S2VX {
	Sprites::Sprites(const std::vector<Sprite*>& pSprites)
		: sprites{ pSprites } {}
	void Sprites::draw(const Camera& camera) {
		for (auto active : actives) {
			sprites[active]->draw(camera);
		}
	}
	void Sprites::update(const int time) {
		for (auto active = actives.begin(); active != actives.end(); ) {
			if (sprites[*active]->getEnd() <= time) {
				active = actives.erase(active);
			}
			else {
				++active;
			}
		}
		while (nextActive != sprites.size() && sprites[nextActive]->getStart() <= time) {
			actives.insert(nextActive++);
		}
		for (const auto active : actives) {
			sprites[active]->update(time);
		}
	}
}
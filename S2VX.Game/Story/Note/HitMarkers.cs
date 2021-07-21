using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class HitMarkers : CompositeDrawable {

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;

        // Exposure for use with tests only
        public List<HitMarker> Markers {
            get {
                List<HitMarker> list = new();
                foreach (HitMarker marker in InternalChildren) {
                    list.Add(marker);
                }
                return list;
            }
        }

        public void AddMarker(Vector2 position, Color4 color, double time, float alpha = 0.8f) =>
            AddInternal(new HitMarker {
                Coordinates = position,
                Colour = color,
                SpawnTime = time,
                MarkerAlpha = alpha
            });

        protected override void Update() {
            var markersToRemove = new List<HitMarker>();

            foreach (HitMarker marker in InternalChildren) {
                if (marker.UpdateMarker()) {
                    markersToRemove.Add(marker);
                }
            }

            foreach (var marker in markersToRemove) {
                RemoveInternal(marker);
            }
        }

        public void Reset() => ClearInternal();
    }
}

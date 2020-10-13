using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game.Story {
    public class Camera : Drawable {

        private object PositionLock { get; set; }
        private object ScaleLock { get; set; }
        private object RotateLock { get; set; }

        [BackgroundDependencyLoader]
        private void Load() => Scale = new Vector2(0.1f);

        /// <summary>
        /// Sets value only if the requestor owns the lock
        /// </summary>
        /// <param name="requestor"></param>
        /// <param name="value"></param>
        public void SetPosition(object requestor, Vector2 value) {
            if (PositionLock == requestor) {
                Position = value;
            }
        }

        /// <summary>
        /// Sets value only if the requestor owns the lock
        /// </summary>
        /// <param name="requestor"></param>
        /// <param name="value"></param>
        public void SetScale(object requestor, Vector2 value) {
            if (ScaleLock == requestor) {
                Scale = value;
            }
        }

        /// <summary>
        /// Sets value only if the requestor owns the lock
        /// </summary>
        /// <param name="requestor"></param>
        /// <param name="value"></param>
        public void SetRotation(object requestor, float value) {
            if (RotateLock == requestor) {
                Rotation = value;
            }
        }

        /// <summary>
        /// Takes the lock and registers the owner if it is free
        /// </summary>
        /// <param name="requestor"></param>
        public void TakeCameraPositionLock(object requestor) {
            if (PositionLock == null) {
                PositionLock = requestor;
            }
        }

        /// <summary>
        /// Takes the lock and registers the owner if it is free
        /// </summary>
        /// <param name="requestor"></param>
        public void TakeCameraScaleLock(object requestor) {
            if (ScaleLock == null) {
                ScaleLock = requestor;
            }
        }

        /// <summary>
        /// Takes the lock and registers the owner if it is free
        /// </summary>
        /// <param name="requestor"></param>
        public void TakeCameraRotationLock(object requestor) {
            if (RotateLock == null) {
                RotateLock = requestor;
            }
        }

        /// <summary>
        /// Releases the lock if the requestor is the owner
        /// </summary>
        /// <param name="requestor"></param>
        public void ReleaseCameraPositionLock(object requestor) {
            if (PositionLock == requestor) {
                PositionLock = null;
            }
        }

        /// <summary>
        /// Releases the lock if the requestor is the owner
        /// </summary>
        /// <param name="requestor"></param>
        public void ReleaseCameraScaleLock(object requestor) {
            if (ScaleLock == requestor) {
                ScaleLock = null;
            }
        }

        /// <summary>
        /// Releases the lock if the requestor is the owner
        /// </summary>
        /// <param name="requestor"></param>
        public void ReleaseCameraRotationLock(object requestor) {
            if (RotateLock == requestor) {
                RotateLock = null;
            }
        }
    }
}

using UnityEngine;

namespace Movers
{
    public class FlyPlayerMover : ExpiringPlayerMover
    {
        protected override float LifeTime => 10;
        
        private float flyHeight;
        
        public FlyPlayerMover(BasePlayerMover _prevMover, float _flyHeight)
        {
            flyHeight = _flyHeight;
            ExpirationCountdown();
        }
        
        protected override float PointHeight(Vector3 _point, Vector2 _size, float _deltaTime)
        {
            return flyHeight;
        }
    }
}
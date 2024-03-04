namespace Movers
{
    public class FastPlayerMover : ExpiringPlayerMover
    {
        protected override float Speed => 20f;
        protected override float LifeTime => 10;

        public FastPlayerMover(BasePlayerMover _prevMover)
        {
            ExpirationCountdown();
        }
    }
}
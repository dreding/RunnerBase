namespace Movers
{
    public class SlowPlayerMover : ExpiringPlayerMover
    {
        protected override float Speed => 5f;
        protected override float LifeTime => 10;
        
        public SlowPlayerMover(BasePlayerMover _prevMover)
        {
            ExpirationCountdown();
        }
    }
}
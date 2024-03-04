using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Movers
{
    public abstract class ExpiringPlayerMover : BasePlayerMover
    {
        protected virtual float LifeTime => -1;
        private CancellationTokenSource disableCancellation;
        
        public Action<BasePlayerMover> onMoverExpired;
        
        protected async void ExpirationCountdown()
        {
            disableCancellation = new CancellationTokenSource();
            if (LifeTime > 0)
            {
                await UniTask.Delay((int)(LifeTime * 1000), DelayType.Realtime, PlayerLoopTiming.Update, disableCancellation.Token).SuppressCancellationThrow();
                onMoverExpired?.Invoke(this);
            }
        }

        public void Dispose()
        {
            disableCancellation.Cancel();
            onMoverExpired = null;
        }
    }
}
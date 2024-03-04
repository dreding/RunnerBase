using System;

namespace Data
{
    public class PlayerData
    {
        public int Coins { get; private set; }
        public float Distance { get; private set; }
        
        public Action onPlayerDataChanges;

        public void ChangeCoinsAmount(int _changeAmount)
        {
            Coins += _changeAmount;
            onPlayerDataChanges?.Invoke();
        }

        public void ChangeDistance(float _changeAmount)
        {
            Distance += _changeAmount;
            onPlayerDataChanges?.Invoke();
        }
    }
}
using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class BonusData
    {
        public BonusType type;
        public ObjectViewData bonusView;
        public int bonusChance;
    }
}

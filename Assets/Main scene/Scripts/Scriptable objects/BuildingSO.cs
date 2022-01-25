using System.Collections.Generic;
using UnityEngine;

namespace GAME
{
    [CreateAssetMenu(fileName ="BuildingSO", menuName ="GAME/Building SO")]
    public class BuildingSO : ScriptableObject
    {
        public float productionTime = 10f;

        public List<ItemSO> inputItems;
        public List<ItemSO> outputItems;

    }
}

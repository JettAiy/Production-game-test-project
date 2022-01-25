using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "GAME/Item SO")]
    public class ItemSO : ScriptableObject
    {
        public GameObject prefab;
    }
}

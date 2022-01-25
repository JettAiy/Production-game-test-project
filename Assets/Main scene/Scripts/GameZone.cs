using System;
using UnityEngine;

namespace GAME.WORLD
{
    public class GameZone : MonoBehaviour
    {

        public event Action<Collider> ZoneEnterTrigger;
        public event Action<Collider> ZoneStayTrigger;
        public event Action<Collider> ZoneExitTrigger;

        private void OnTriggerEnter(Collider other)
        {
            ZoneEnterTrigger?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            ZoneStayTrigger?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ZoneExitTrigger?.Invoke(other);
        }

    }
}

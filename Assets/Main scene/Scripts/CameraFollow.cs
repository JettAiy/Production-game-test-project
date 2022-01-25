using UnityEngine;

namespace GAME
{
    public class CameraFollow : MonoBehaviour
    {

        [SerializeField] private Transform _followObject;

        private Vector3 _offset;

        private void Awake()
        {
            _offset = transform.position - _followObject.position;
        }

        private void LateUpdate()
        {
            transform.position = _followObject.position + _offset;
        }

    }
}

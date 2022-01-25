
using System;
using UnityEngine;

namespace GAME.WORLD
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemSO;

        public ItemSO ItemSO => _itemSO;

        public string ID { get; private set; }

        private Action<Item> _storageCallback;

        private Transform _endPoint;
        private bool isTransfering;

        private float _flightTime = 0.5f;
        private float _speed;

        private void Awake()
        {
            ID = Guid.NewGuid().ToString();   
        }

        public void Transfer(Transform endPoint, Action<Item> storageCallback)
        {
            _storageCallback = storageCallback;
            _endPoint = endPoint;
            _speed = Vector3.Distance(transform.position, endPoint.transform.position) / _flightTime;
            transform.parent = null;
            isTransfering = true;
        }

        private void Update()
        {
            if (isTransfering) Move();
        }

        private void Move()
        { 
            transform.position = Vector3.Lerp(transform.position, _endPoint.position, Time.deltaTime * _speed);

            float dist = Vector3.Distance(_endPoint.position, transform.position);

            if (dist < 0.2f)
            {
                transform.parent = _endPoint;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                isTransfering = false;
                
                _storageCallback.Invoke(this);
            }

        }
    }
}

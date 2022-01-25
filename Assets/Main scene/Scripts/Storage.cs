using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.WORLD
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private Transform _storagePoint;
        
        [SerializeField] private int _maxStorageAmount;
        
        public List<Item> StorageItems { get; private set; }
        public int MaxCount => _maxStorageAmount;

        public bool isMaxCount => StorageItems.Count >= _maxStorageAmount;

        public int Count => StorageItems.Count;

        public bool inTransfer;

        public bool isTransfer
        {
            get
            {
                return inTransfer && StorageItems.Count > 0;
            }
        }

        private void Awake()
        {
            StorageItems = new List<Item>(_maxStorageAmount);
        }

        #region CORE
        public bool Add(Item item)
        {
            bool result = false;

            if (!isMaxCount && !HasItem(item) && item != null)
            {
                StorageItems.Add(item);
                item.Transfer(_storagePoint, OnItemTransferComplete);
                inTransfer = true;
                result = true;
            }


            return result;
        }
        public Item GetItemByType(ItemSO itemSO)
        {
            return StorageItems.Find(item=> item.ItemSO == itemSO);
        }
        
        public bool Subtract(Item item)
        {
            bool result = false;

            if (HasItem(item))
            {
                StorageItems.Remove(item);
                result = true;
                RecalculateItemPositions();
            }
            
            return result;
        }

        public Item GetLastItem()
        {
            if (StorageItems.Count > 0)
                return StorageItems[StorageItems.Count - 1];
            else
                return null;
        }
        #endregion


        #region UTILS
        private bool HasItem(Item _item)
        {
            Item current = StorageItems.Find(item => item.ID.Equals(_item.ID));

            return current != null;
        }

        private void OnItemTransferComplete(Item item)
        {
            inTransfer = false;            
            RecalculateItemPositions();
        }

        private void RecalculateItemPositions()
        {
            Vector3 startPosition = Vector3.zero;

            foreach (var item in StorageItems)
            {
                if (item == null) continue;

                startPosition.y += item.transform.localScale.y / 2;
                item.transform.localPosition = startPosition;
            }
        }
        #endregion


        #region STATIC
        public static void TransferItems(Storage sourceStorage, Storage receiverStorage)
        {
            Item transferedItem = sourceStorage.GetLastItem();

            if (transferedItem != null && receiverStorage.Add(transferedItem))
            {
                sourceStorage.Subtract(transferedItem);
            }
        }
        #endregion
    }
}

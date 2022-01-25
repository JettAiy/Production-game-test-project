using GAME.UTILS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GAME.WORLD
{
    public class Building : MonoBehaviour
    {

        [SerializeField] private BuildingSO _buildingSO;

        [Space]
        [SerializeField] private GameZone _inputZone;
        [SerializeField] private GameZone _outputZone;

        [Space]
        [SerializeField] private TextMeshProUGUI _infoText;

        public BuildingSO BuildingSO => _buildingSO;

        public Storage InputStorage;
        public Storage OutputStorage;
        public Timer ProductionTimer { get; private set; }

        //transfering
        private Storage _sourceStorage;
        private Storage _receiverStorage;

        private string _errorText;

        #region INIT
        private void Awake()
        {
            ProductionTimer = new Timer(ProduceItem, _buildingSO.productionTime);
            ProductionTimer.Start();

            _inputZone.ZoneEnterTrigger += InputZoneEnterTrigger;
            _inputZone.ZoneStayTrigger += InputZoneStayTrigger;
            _inputZone.ZoneExitTrigger += InputZoneExitTrigger;

            _outputZone.ZoneEnterTrigger += OutputZoneEnterTrigger;
            _outputZone.ZoneStayTrigger += OutputZoneStayTrigger;
            _outputZone.ZoneExitTrigger += OutputZoneExitTrigger;
        }
    
        private void OnDestroy()
        {
            _inputZone.ZoneEnterTrigger -= InputZoneEnterTrigger;
            _inputZone.ZoneStayTrigger -= InputZoneStayTrigger;
            _inputZone.ZoneExitTrigger -= InputZoneExitTrigger;

            _outputZone.ZoneEnterTrigger -= OutputZoneEnterTrigger;
            _outputZone.ZoneStayTrigger -= OutputZoneStayTrigger;
            _outputZone.ZoneExitTrigger -= OutputZoneExitTrigger;
        }

        #endregion

        #region EVENTS
        private void InputZoneEnterTrigger(Collider obj)
        {
            
        }

        private void InputZoneStayTrigger(Collider obj)
        {
            Player player = obj.GetComponent<Player>();

            if (player != null)
            {
                _sourceStorage = player.Storage;
                _receiverStorage = InputStorage;
                StartTranfserItems();
            }
        }

        private void InputZoneExitTrigger(Collider obj)
        {
            
        }

        private void OutputZoneEnterTrigger(Collider obj)
        {

        }

        private void OutputZoneStayTrigger(Collider obj)
        {
            Player player = obj.GetComponent<Player>();

            if (player != null)
            {
                _sourceStorage = OutputStorage;
                _receiverStorage = player.Storage;
                StartTranfserItems();
            }
        }

        private void OutputZoneExitTrigger(Collider obj)
        {
            
        }

        
        #endregion

        #region CORE

        private void Update()
        {
            if (ProductionTimer != null) ProductionTimer.Tick();

            UpdateTextInfo();
        }


        private void ProduceItem()
        {
            _errorText = "";

            if (OutputStorage.isMaxCount)
            {
                _errorText = "max. output!";
                return;
            }

            List<Item> consumableItems = new List<Item>();

            //check that we have items from input storage
            foreach (var inputItem in _buildingSO.inputItems)
            {
                Item item = InputStorage.GetItemByType(inputItem);

                if (item != null)
                {
                    consumableItems.Add(item);
                }
                else
                {
                    _errorText += $"No {inputItem.name}!"; 
                }
            }

            //if we have same amount items to subtract - then produce new item
            if (_buildingSO.inputItems.Count == consumableItems.Count)
            {
                foreach (var outputItem in _buildingSO.outputItems)
                {
                    Item item = Instantiate(outputItem.prefab).GetComponent<Item>();

                    item.transform.parent = _outputZone.transform;

                    float y = OutputStorage.Count * (item.transform.localScale.y / 2);

                    item.transform.localPosition = new Vector3(0, y, 0);

                    OutputStorage.Add(item);
                }

                foreach (var item in consumableItems)
                {
                    InputStorage.Subtract(item);
                    Destroy(item.gameObject);
                }
            } 

        }

        #endregion

        #region UI
        private void UpdateTextInfo()
        {
            string text = string.IsNullOrEmpty(_errorText) ? $"Cycle: {ProductionTimer.CurrentTime.ToString("N1")}" :  "STOPPED";
            text += $"\r\n {InputStorage.Count} / {OutputStorage.Count}";
            text += string.IsNullOrEmpty(_errorText) ? "" : $"\r\n Error: {_errorText}";
            _infoText.text = text;
        }
        #endregion

        #region UTILS
        private void StartTranfserItems()
        {
            if (_receiverStorage.isTransfer) return;
            Storage.TransferItems(_sourceStorage, _receiverStorage);
        }
        #endregion
    }
}

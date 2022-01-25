using System;
using UnityEngine;

namespace GAME.UTILS
{
    [Serializable]
    public class Timer
    {

        private Action _callbackAction;
        private float _maxTime;

        public float CurrentTime { get; private set; }
        public bool isStop { get; private set; } = true;
        public float CurrentPercent
        {
            get
            {
                return CurrentTime / _maxTime;
            }
        }
        
        public Timer(Action callbackAction, float maxTime)
        {
            _callbackAction = callbackAction;
            _maxTime = maxTime;
            CurrentTime = maxTime;
        }

        public void Start()
        {
            isStop = false;
        }

        public void Stop()
        {
            isStop = true;
        }

        public void SetTime(float maxTime)
        {
            _maxTime = maxTime;
            Reset();
        }

        public void SetCurrentTime(float currentTime)
        {
            this.CurrentTime = currentTime;
        }

        public void Reset()
        {
            CurrentTime = _maxTime;
        }

        public void Tick()
        {
            if (isStop) return;

            CurrentTime -= Time.deltaTime;

            if (CurrentTime <= 0)
            {
                CurrentTime += _maxTime;

                _callbackAction?.Invoke();
            }
        }


        
    }
}

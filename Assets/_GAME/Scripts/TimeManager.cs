namespace _GAME.Scripts
{
    using UnityEngine;
    using System;
    using Tenkoku.Core;

    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [Header("Time Settings")] [Tooltip("Tốc độ thời gian trôi qua. 1 = thời gian thực. 60 = 1 phút đời thực là 1 giờ trong game.")] public float timeMultiplier = 60f;

        [Header("Time Periods (24-hour format)")] public int morningStartHour = -8;
        public                                           int dayStartHour     = -6;
        public                                           int eveningStartHour = 5;
        public                                           int nightStartHour   = 10;

        private TenkokuModule _tenkokuModule;
        private float         _totalGameSeconds = 0f;

        public TimeOfDay CurrentTimeOfDay { get; private set; }

        public static event Action<TimeOfDay> OnTimeOfDayChanged;

        public enum TimeOfDay
        {
            Morning,
            Day,
            Evening,
            Night
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            GameObject tenkokuObject = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObject != null)
            {
                _tenkokuModule = tenkokuObject.GetComponent<TenkokuModule>();
                InitializeTime();
            }
            else
            {
                Debug.LogError("TimeManager không tìm thấy 'Tenkoku DynamicSky' trong scene!");
                this.enabled = false;
            }
        }

        void InitializeTime()
        {
            _totalGameSeconds = _tenkokuModule.currentHour * 3600f + _tenkokuModule.currentMinute * 60f + _tenkokuModule.currentSecond;
            UpdateTimeOfDayState(true);
        }

        void Update()
        {
            if (_tenkokuModule == null) return;

            _totalGameSeconds += Time.deltaTime * timeMultiplier;

            if (_totalGameSeconds >= 86400f)
            {
                _totalGameSeconds = 0;
            }

            _tenkokuModule.currentHour   = (int)(_totalGameSeconds / 3600f);
            _tenkokuModule.currentMinute = (int)((_totalGameSeconds % 3600f) / 60f);
            _tenkokuModule.currentSecond = (int)(_totalGameSeconds % 60f);

            UpdateTimeOfDayState();
        }

        void UpdateTimeOfDayState(bool forceUpdate = false)
        {
            TimeOfDay newTimeOfDay = GetTimeOfDay(_tenkokuModule.currentHour);

            if (newTimeOfDay != CurrentTimeOfDay || forceUpdate)
            {
                CurrentTimeOfDay = newTimeOfDay;
                Debug.Log("Thời gian đã chuyển sang: " + CurrentTimeOfDay);

                OnTimeOfDayChanged?.Invoke(CurrentTimeOfDay);
            }
        }

        TimeOfDay GetTimeOfDay(int hour)
        {
            if (hour >= morningStartHour && hour < dayStartHour) return TimeOfDay.Morning;
            if (hour >= dayStartHour && hour < eveningStartHour) return TimeOfDay.Day;
            if (hour >= eveningStartHour && hour < nightStartHour) return TimeOfDay.Evening;
            return TimeOfDay.Night;
        }
    }
}
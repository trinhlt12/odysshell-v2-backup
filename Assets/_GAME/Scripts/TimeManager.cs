using UnityEngine;
using System;
using Tenkoku.Core;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Time Settings")]
    [Tooltip("Tốc độ thời gian trôi qua. 1 = thời gian thực. 60 = 1 phút đời thực là 1 giờ trong game.")]
    public float timeMultiplier = 60f;

    public TenkokuModule tenkokuModule;
    [Header("Time Periods (24-hour format)")]
    public int morningStartHour = -8;
    public int dayStartHour = -6;
    public int eveningStartHour = 5;
    public int nightStartHour = 10;

    private TenkokuModule _tenkokuModule;
    private float _totalGameSeconds = 0f;

    public TimeOfDay CurrentTimeOfDay { get; private set; }

    [Header("Day Tracking")]
    [Tooltip("Số ngày hiện tại trong game.")]
    public int currentDay = 1;

    public static event Action<TimeOfDay> OnTimeOfDayChanged;
    public static event Action<int> OnDayChanged;

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

        SetTimeOfDay(TimeOfDay.Morning);
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
            _totalGameSeconds -= 86400f;
            currentDay++;
            Debug.Log("Một ngày mới đã bắt đầu! Bây giờ là Ngày " + currentDay);
            OnDayChanged?.Invoke(currentDay);
        }

        UpdateTimeFromTotalSeconds();
        UpdateTimeOfDayState();
    }
    public void SetTimeOfDay(TimeOfDay timeOfDay)
    {
        int targetHour = 0;

        switch (timeOfDay)
        {
            case TimeOfDay.Morning:
                targetHour = morningStartHour;
                break;
            case TimeOfDay.Day:
                targetHour = dayStartHour;
                break;
            case TimeOfDay.Evening:
                targetHour = eveningStartHour;
                break;
            case TimeOfDay.Night:
                targetHour = nightStartHour;
                break;
        }

        Debug.Log("Đặt lại thời gian thành: " + timeOfDay + " (" + targetHour + "h)");

        _totalGameSeconds = targetHour * 3600f;

        UpdateTimeFromTotalSeconds();
        UpdateTimeOfDayState(true);
    }
    private void UpdateTimeFromTotalSeconds()
    {
        if (_tenkokuModule == null) return;

        _tenkokuModule.currentHour = (int)(_totalGameSeconds / 3600f);
        _tenkokuModule.currentMinute = (int)((_totalGameSeconds % 3600f) / 60f);
        _tenkokuModule.currentSecond = (int)(_totalGameSeconds % 60f);
    }


    void UpdateTimeOfDayState(bool forceUpdate = false)
    {
        if (_tenkokuModule == null) return;

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
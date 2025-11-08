using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CinemachineFreeLookController : MonoBehaviour
{
    [Header("Tốc độ và độ nhạy")] [Tooltip("Tốc độ xoay camera theo trục ngang (trái/phải)")] public float lookSpeedX = 200f;
    [Tooltip("Tốc độ xoay camera theo trục dọc (lên/xuống)")]                                 public float lookSpeedY = 2f;
    [Tooltip("Tốc độ phóng to / thu nhỏ")]                                                    public float zoomSpeed  = 5f;

    [Header("Giới hạn Zoom")] [Tooltip("Bán kính zoom gần nhất")] public float minZoomRadius = 2f;
    [Tooltip("Bán kính zoom xa nhất")]                            public float maxZoomRadius = 15f;

    private CinemachineFreeLook _freeLookCamera;
    private bool                _cameraInputIsActive = true;

    private void Awake()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    private void Update()
    {
        HandleCursorState();

        if (!_cameraInputIsActive)
        {
            return;
        }

        HandleCameraRotation();
        HandleCameraZoom();
    }

    private void HandleCursorState()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState     = CursorLockMode.None;
            Cursor.visible       = true;
            _cameraInputIsActive = false;
        }
        else
        {
            Cursor.lockState     = CursorLockMode.Locked;
            Cursor.visible       = false;
            _cameraInputIsActive = true;
        }
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _freeLookCamera.m_XAxis.Value += mouseX * lookSpeedX * Time.deltaTime;

        _freeLookCamera.m_YAxis.Value += mouseY * lookSpeedY * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Approximately(scrollWheel, 0f))
        {
            return;
        }
        float currentRadius = _freeLookCamera.m_Orbits[1].m_Radius;
        float newRadius     = currentRadius - scrollWheel * zoomSpeed;

        newRadius                            = Mathf.Clamp(newRadius, minZoomRadius, maxZoomRadius);
        _freeLookCamera.m_Orbits[0].m_Radius = newRadius * 1.2f;
        _freeLookCamera.m_Orbits[1].m_Radius = newRadius;
        _freeLookCamera.m_Orbits[2].m_Radius = newRadius * 0.8f;
    }
}
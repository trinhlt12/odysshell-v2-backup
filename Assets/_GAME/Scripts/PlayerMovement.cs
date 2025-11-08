using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator            _animator;
    private Transform           cam;

    public float speed      = 2;
    public float gravity    = -9.18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float     groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool    isGrounded;

    private readonly int _horizontalParam = Animator.StringToHash("Horizontal");
    private readonly int _verticalParam   = Animator.StringToHash("Vertical");
    private readonly int _isMovingParam   = Animator.StringToHash("isMoving");

    private readonly int _lastHorizontalParam = Animator.StringToHash("LastHorizontal");
    private readonly int _lastVerticalParam   = Animator.StringToHash("LastVertical");

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main.transform;

        var spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
            controller.enabled = false;

            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;

            controller.enabled = true;

            Debug.Log("Player đã được dịch chuyển đến SpawnPoint.");
        }
    }

    void Update()
    {
        if (cam == null)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight   = cam.right;
        camForward.y = 0;
        camForward.Normalize();

        transform.forward = camForward;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move.normalized * speed * Time.deltaTime);

        _animator.SetFloat(_horizontalParam, x);
        _animator.SetFloat(_verticalParam, z);
        bool isCurrentlyMoving = move.sqrMagnitude > 0.01f;
        _animator.SetBool(_isMovingParam, isCurrentlyMoving);
        if (isCurrentlyMoving)
        {
            _animator.SetFloat(_lastHorizontalParam, x);
            _animator.SetFloat(_lastVerticalParam, z);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
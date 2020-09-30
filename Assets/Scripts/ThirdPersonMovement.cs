using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartSprinting = delegate { };
    public event Action Airborne = delegate { };
    public event Action StartJumping = delegate { };
    public event Action Landing = delegate { };

    public List<Collider> RagdollParts = new List<Collider>();
    Animator animator = null;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] LevelReset levelreset;

    public MovementState MovementType;

    public float speed = 18f;
    public float sprintspeed = 25f;
    public float _gravity = -9.81f;
    public float _jumpHeight = 3f;
    public float turnSmoothTime = 0.1f;

    Vector3 _velocity;
    public Transform _groundCheck;
    public float _groundDistance = 0.4f;
    public LayerMask _groundMask;

    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isSprinting = false;
    bool _isGrounded = false;

    public enum MovementState
    {
        Idle, Running, Sprinting, Jumping, Punching
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        levelreset = GetComponent<LevelReset>();
        SetRagdollParts();
        Idle?.Invoke();
    }

    void Update()
    {
        Movement();
        Gravity();
        Jump();
        Sprint();
        MovementStateMachine();
        CursorLock();
    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MovementStateMachine()
    {
        //Movement State Machine
        if (MovementType == MovementState.Idle)
        {
            CheckIfStoppedMoving();
        }

        if (MovementType == MovementState.Running)
            if (_isMoving == true)
            {
                CheckIfStartedMoving();
                CheckIfStoppedSprintingIntoRunning();
            }

        if (MovementType == MovementState.Sprinting)
            if (_isMoving == true)
            {
                CheckIfSprinting();
            }

        if (MovementType == MovementState.Sprinting)
            if (_isMoving == false)
            {
                CheckIfStoppedSprinting();
            }
    }
    
    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                RagdollParts.Add(c);
            }
        }
    }


    public void TurnOnRagdoll()
    {
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        animator.enabled = false;
        animator.avatar = null;
        foreach(Collider c in RagdollParts)
        {
            c.isTrigger = false;
            //c.attachedRigidbody.velocity = Vector3.zero;
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        else
        {
            CheckIfStoppedMoving();
        }
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            CheckIfStartedJumping();
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MovementType = MovementState.Sprinting;
            speed = sprintspeed;
        }
        else
        {
            speed = 18f;
            if (_isMoving == false)
            {
                MovementType = MovementState.Idle;
            }

            if (_isMoving == true)
            {
                MovementType = MovementState.Running;
            }
            
        }
    }
 
    

    private void Gravity()
    {
        if (levelreset._isdead == false)
        {
            //Gravity
            _velocity.y += _gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);

            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }
        if (levelreset._isdead == true)
        {
            this.enabled = false;
        }
        
       
    }

    private void CheckIfStartedMoving()
    {
        if(_isMoving == false)
        {
            StartRunning?.Invoke();
            Debug.Log("Started");
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        _isMoving = false;
    }
    private void CheckIfSprinting()
    {
        if (_isSprinting == false)
        {
            StartSprinting?.Invoke();
            Debug.Log("Sprinting");
        }
        _isSprinting = true;
    }
    private void CheckIfStoppedSprinting()
    {
        if (_isSprinting == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        _isSprinting = false;
    }

    private void CheckIfStoppedSprintingIntoRunning()
    {
        if (_isSprinting == true)
        {
            StartRunning?.Invoke();
            Debug.Log("Stopped");
        }
        _isSprinting = false;
    }

    private void CheckIfStartedJumping()
    {
        if(_isGrounded == true)
        {
            StartJumping?.Invoke();
            Debug.Log("Jumping");
        }
    }

}

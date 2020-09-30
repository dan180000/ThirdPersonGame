using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public float AttackDistance;
    public float DetectionDistance;
    public Vector3 AttackPoint;
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartSprinting = delegate { };
    public event Action Airborne = delegate { };
    public event Action StartJumping = delegate { };
    public event Action Landing = delegate { };
    public event Action Punching = delegate { };
    public event Action StartDamage = delegate { };
    public List<Collider> RagdollParts = new List<Collider>();
    public List<Collider> CollidingParts = new List<Collider>();
    Animator animator = null;

    public GameObject healthBarUI;
    public Slider slider;

    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;

    public MovementState MovementType;

    public float speed = 18f;
    public float sprintspeed = 25f;
    public float _gravity = -9.81f;
    public float _jumpHeight = 3f;
    public float turnSmoothTime = 0.1f;
    public int HitPoints;
    public int MaxHitPoints = 100;
    public bool IsAttacking = false;
    public AudioClip PunchSound;
    private AudioSource AS;

    Vector3 _velocity;
    public Transform _groundCheck;
    public float _groundDistance = 0.4f;
    public LayerMask _groundMask;

    float turnSmoothVelocity;
    bool _isMoving = false;
    bool _isGrounded = false;
    

    public enum MovementState
    {
        Idle, Running, Sprinting, Jumping, Punching
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRagdollParts();
        Idle?.Invoke();
        AS = GetComponent<AudioSource>();

        HitPoints = MaxHitPoints;
        
    }

    void Update()
    {
        Gravity();

        slider.value = CalculateHealth();

        //Movement State Machine
        if (MovementType == MovementState.Idle)
        {
            CheckIfStoppedMoving();
        }
    }

    float CalculateHealth()
    {
        return HitPoints;
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }


    public void TurnOnRagdoll()
    {
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        animator.enabled = false;
        animator.avatar = null;
        foreach (Collider c in RagdollParts)
        {
            c.isTrigger = false;
            //c.attachedRigidbody.velocity = Vector3.zero;
        }
    }

  

    private void OnTriggerEnter(Collider col)
    {
        if (RagdollParts.Contains(col))
        {
            return;
        }

        CharacterController control = col.transform.root.GetComponent<CharacterController>();
        if (control == null)
        {
            return;
        }

        if (col.gameObject == control.gameObject)
        {
            return;
        }

        if (!CollidingParts.Contains(col))
        {
            CollidingParts.Add(col);
            Debug.Log("Hit!");
            HitPoints -= 15;
            AS.PlayOneShot(PunchSound);
           
            if(HitPoints <= 0)
            {
                Die();
                healthBarUI.SetActive(false);
            }
        }
        else 
        {
            CheckIfStoppedMoving();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (CollidingParts.Contains(col))
        {
            CollidingParts.Remove(col);
        }
    }

    private void Die()
    { 
        TurnOnRagdoll();
        this.enabled = false;
        Debug.Log("Enemy Defeated!");
    }


    private void Gravity()
    {
        //Gravity
        _velocity.y += _gravity * Time.deltaTime;
        

        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

    }

    private void CheckIfStartedMoving()
    {
        if (_isMoving == false)
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

}

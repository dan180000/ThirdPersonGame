using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    public event Action Punching = delegate { };
    public event Action Jabbing = delegate { };
    public event Action Hooking = delegate { };
    public event Action Kicking = delegate { };
    Animator animator = null;

    public enum ComboState
    {
        none,
        punch1,
        hook,
        punch2,
        kick
    }

    private bool activateResetTimer;
    private float default_Combo_Timer = 0.4f;
    private float current_Combo_Timer;

    private ComboState current_Combo_State;

    bool _punch = false;
    bool _hook = false;
    bool _jab = false;
    bool _kick = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        current_Combo_Timer = default_Combo_Timer;
        current_Combo_State = ComboState.none;
        current_Combo_State = (ComboState)1;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttack();
        ResetComboState();
    }

    void ComboAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (current_Combo_State == ComboState.punch2)
            {
                return;
            }
                
            current_Combo_State++;
            activateResetTimer = true;
            current_Combo_Timer = default_Combo_Timer;
            if(current_Combo_State == ComboState.punch1)
            {
                IsPunching();
            }
            if (current_Combo_State == ComboState.hook)
            {
                IsHooking();
            }
            if (current_Combo_State == ComboState.punch2)
            {
                IsJabbing();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(current_Combo_State == ComboState.kick || current_Combo_State == ComboState.punch2)
            {
                return;
            }

            if(current_Combo_State == ComboState.none || 
                current_Combo_State == ComboState.punch1 || 
                current_Combo_State == ComboState.hook)
            {
                current_Combo_State = ComboState.kick;
            }
            activateResetTimer = true;
            current_Combo_Timer = default_Combo_Timer;

            if(current_Combo_State == ComboState.kick)
            {
                IsKicking();
            }  
        }
    }

    void ResetComboState()
    {
        if(activateResetTimer)
        {
            current_Combo_Timer -= Time.deltaTime;
            if(current_Combo_Timer <= 0f)
            {
                current_Combo_State = ComboState.none;
                activateResetTimer = false;
                current_Combo_Timer = default_Combo_Timer;
                Debug.Log("Combo Reset");
            }
        }
    }

    private void IsPunching()
    {
        if (_punch == false)
        {
            Punching?.Invoke();
            Debug.Log("Punching");
        }
    }

    private void IsHooking()
    {
        if (_hook == false)
        {
            Hooking?.Invoke();
            Debug.Log("Hooking");
        }
    }

    private void IsJabbing()
    {
        if (_jab == false)
        {
            Jabbing?.Invoke();
            Debug.Log("Jabbing");
        }
    }

    private void IsKicking()
    {
        if (_kick == false)
        {
            Kicking?.Invoke();
            Debug.Log("Kicking");
        }
    }
}

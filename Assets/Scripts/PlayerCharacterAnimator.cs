using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdpersonMovement = null;
    [SerializeField] PlayerAttack _playerAttack = null;

    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string LandingState = "Landing";
    const string SprintState = "Sprint";
    const string PunchState = "Punch";
    const string HookState = "Hook";
    const string JabState = "Jab";
    const string KickState = "Kick";

    Animator _animator = null;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }
    private void OnStartSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    private void OnStartJump()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnAirborne()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnLanding()
    {
        _animator.CrossFadeInFixedTime(LandingState, .2f);
    }

    private void OnPunching()
    {
        _animator.CrossFadeInFixedTime(PunchState, .2f);
    }

    private void OnHooking()
    {
        _animator.CrossFadeInFixedTime(HookState, .2f);
    }
    private void OnJabbing()
    {
        _animator.CrossFadeInFixedTime(JabState, .2f);
    }

    private void OnKicking()
    {
        _animator.CrossFadeInFixedTime(KickState, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _thirdpersonMovement.Idle += OnIdle;
        _thirdpersonMovement.StartRunning += OnStartRunning;
        _thirdpersonMovement.StartSprinting += OnStartSprinting;
        _thirdpersonMovement.StartJumping += OnStartJump;
        _thirdpersonMovement.Airborne += OnAirborne;
        _thirdpersonMovement.Landing += OnLanding;
        _playerAttack.Punching += OnPunching;
        _playerAttack.Hooking += OnHooking;
        _playerAttack.Jabbing += OnJabbing;
        _playerAttack.Kicking += OnKicking;

    }

    private void OnDisable()
    {
        _thirdpersonMovement.Idle -= OnIdle;
        _thirdpersonMovement.StartRunning -= OnStartRunning;
        _thirdpersonMovement.StartSprinting -= OnStartSprinting;
        _thirdpersonMovement.StartJumping -= OnStartJump;
        _thirdpersonMovement.Airborne -= OnAirborne;
        _thirdpersonMovement.Landing -= OnLanding;
        _playerAttack.Punching -= OnPunching;
        _playerAttack.Hooking -= OnHooking;
        _playerAttack.Jabbing -= OnJabbing;
        _playerAttack.Kicking -= OnKicking;
    }
}

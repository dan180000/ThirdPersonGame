using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelReset : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdpersonmovement = null;
    [SerializeField] HealthScript _healthScript = null;

    public bool _isdead = false;
    float deathtime = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        ResetLevel();
        Exit();

        if (_isdead == true)
        {
            deathtime -= Time.deltaTime;
        }
    }

    public void Die()
    {
        if (_healthScript.playerdead == true)
        {
            _thirdpersonmovement.TurnOnRagdoll();
            _thirdpersonmovement.enabled = false;
            Debug.Log("You Died!");
            _isdead = true;
        }
    }

    private void ResetLevel()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (deathtime <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Exit()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] LevelReset levelreset;

    public List<Collider> RagdollParts = new List<Collider>();
    public List<Collider> CollidingParts = new List<Collider>();

    public GameObject healthBarUI;
    public GameObject DeathUI;
    public Slider slider;

    public AudioClip PunchSound;
    private AudioSource AS;

    [SerializeField] float playerhealth;
    [SerializeField] float maxplayerHealth = 100f;
    public bool playerdead = false;
    private CapsuleCollider CC;
    public GameObject HurtImage;
    // Start is called before the first frame update
    void Start()
    {
        levelreset.GetComponent<LevelReset>();
        CC = GetComponent<CapsuleCollider>();
        playerhealth = maxplayerHealth;
        DeathUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        slider.value = CalculateHealth();
    }

    float CalculateHealth()
    {
        return playerhealth;
    }


    void Die()
    {
        if (playerhealth <= 0)
        {
            playerdead = true;
            CC.enabled = false;
            DeathUI.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HazardObject")
        {
            playerhealth -= 25;

            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "DeathPit")
        {
            playerhealth = 0;
        } 
    }
}

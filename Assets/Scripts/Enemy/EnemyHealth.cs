using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public Slider healthSlider;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        //healthSlider.transform.ro = Quaternion.identity;
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        Debug.Log("Damage taken");
        if(isDead)
            return;

        //enemyAudio.Play ();

        currentHealth -= amount;
        currentHealth = currentHealth >= 0 ? currentHealth : 0;
        healthSlider.value = currentHealth;
        
        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
            return;
        }
        healthSlider.image.color = Color.Lerp(Color.red, Color.green, healthSlider.normalizedValue);// new Color(256 * (1 - healthSlider.normalizedValue), 256 * healthSlider.normalizedValue, .0f);
    }


    void Death ()
    {
        isDead = true;

        anim.SetTrigger ("Dead");

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}

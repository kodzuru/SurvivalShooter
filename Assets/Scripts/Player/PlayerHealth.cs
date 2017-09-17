using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
public int startingHealth = 100;// The amount of health the player starts the game with.
    public int currentHealth;// The current health the player has.
    public Slider healthSlider;// Reference to the UI's health bar.
    public Image damageImage;// Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;// The audio clip to play when the player dies.
    public float flashSpeed = 5f;// The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);// The colour the damageImage is set to, to flash.


    Animator anim;// Reference to the Animator component.
    AudioSource playerAudio;// Reference to the AudioSource component.
    PlayerMovement playerMovement;// Reference to the player's movement.
    PlayerShooting playerShooting;// Reference to the PlayerShooting script.
    bool isDead;// Whether the player is dead.
    bool damaged;// True when the player gets damaged.


    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();

        currentHealth = startingHealth;// Set the initial health of the player.
    }


    void Update ()
    {
        if(damaged)// If the player has just been damaged...
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;// Set the damaged flag so the screen will flash.

        currentHealth -= amount; // Reduce the current health by the damage amount.

        healthSlider.value = currentHealth;// Set the health bar's value to the current health.

        playerAudio.Play ();// Play the hurt sound effect.

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            Death (); // ... it should die.
        }
    }


    void Death ()
    {
        isDead = true;// Set the death flag so this function won't be called again.

        playerShooting.DisableEffects (); // Turn off any remaining shooting effects.

        anim.SetTrigger ("Die"); // Tell the animator that the player is dead.

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play ();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

     

}

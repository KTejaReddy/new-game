using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float xySpeed = 15f;
    public float xyClamp = 10f; // Limit how far the player can move left/right/up/down
    public float boostMultiplier = 2f;
    public float rotationTilt = 30f;
    public float rotationSpeed = 5f;

    [Header("Effects")]
    public ParticleSystem explosionParticles;
    public AudioSource engineAudio;
    public AudioClip crashSound;
    public AudioClip collectSound;
    private AudioSource sfxAudioSource;

    private float currentSpeedMultiplier = 1f;

    private void Start()
    {
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        if (engineAudio != null)
        {
            engineAudio.Play();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        // Inputs
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        bool isBoosting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift);

        currentSpeedMultiplier = isBoosting ? boostMultiplier : 1f;

        // XY Movement
        Vector3 movement = new Vector3(moveX, moveY, 0f) * xySpeed * Time.deltaTime;
        transform.position += movement;

        // Clamp Position
        float clampedX = Mathf.Clamp(transform.position.x, -xyClamp, xyClamp);
        float clampedY = Mathf.Clamp(transform.position.y, -xyClamp, xyClamp);
        
        // Forward Movement (Constant + Boost)
        float forwardMovement = GameManager.Instance.CurrentSpeed * currentSpeedMultiplier * Time.deltaTime;
        
        transform.position = new Vector3(clampedX, clampedY, transform.position.z + forwardMovement);

        // Tilt Rotation based on input
        Quaternion targetRotation = Quaternion.Euler(-moveY * rotationTilt, moveX * rotationTilt, -moveX * rotationTilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Adjust Engine pitch based on boost
        if (engineAudio != null)
        {
            engineAudio.pitch = isBoosting ? 1.5f : 1.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.IsGameActive) return;

        if (other.CompareTag("Obstacle"))
        {
            Crash();
        }
        else if (other.CompareTag("Collectible"))
        {
            Collect(other.gameObject);
        }
    }

    private void Crash()
    {
        if (explosionParticles != null)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }

        if (crashSound != null)
        {
            sfxAudioSource.PlayOneShot(crashSound);
        }

        if (engineAudio != null)
        {
            engineAudio.Stop();
        }

        GameManager.Instance.GameOver();
        
        // Hide player visually
        GetComponent<MeshRenderer>().enabled = false;
        // Optionally disable children models
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Collect(GameObject collectible)
    {
        GameManager.Instance.AddScore(100f);
        if (collectSound != null)
        {
            sfxAudioSource.PlayOneShot(collectSound);
        }
        collectible.SetActive(false); // Return to pool essentially
    }
}

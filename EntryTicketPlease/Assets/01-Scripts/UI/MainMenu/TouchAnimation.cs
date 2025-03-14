using UnityEngine;

public class ClickParticles : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource pour jouer les sons
    [SerializeField] private AudioClip[] clickSounds; // Tableau contenant 3 sons diff�rents

    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private Camera uiCamera; // Assigne ta cam�ra ici

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetWorldPositionFromUI(Input.mousePosition);
            SpawnEffect(worldPosition);
        }
    }

    private Vector3 GetWorldPositionFromUI(Vector3 uiPosition)
    {
        uiPosition.z = uiCamera.nearClipPlane + 1f; // Ajuste la profondeur pour voir les particules
        return uiCamera.ScreenToWorldPoint(uiPosition);
    }

    private void SpawnEffect(Vector3 position)
    {
        ParticleSystem effect = Instantiate(particleEffect, position, Quaternion.identity);
        effect.Play();

        // Jouer le son de clic
        PlayRandomSound();

        Destroy(effect.gameObject, 2f); // D�truit l'effet apr�s 2s
    }

    private void PlayRandomSound()
    {
        if (audioSource != null && clickSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, clickSounds.Length); // S�lectionne un son al�atoire
            audioSource.PlayOneShot(clickSounds[randomIndex]);
        }
    }
}

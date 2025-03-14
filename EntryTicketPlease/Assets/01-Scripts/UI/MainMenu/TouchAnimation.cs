using UnityEngine;

public class ClickParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private Camera uiCamera; // Assigne ta caméra ici

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
        Destroy(effect.gameObject, 2f); // Détruit l'effet après 2s
    }
}

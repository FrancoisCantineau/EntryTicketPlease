using UnityEngine;
using UnityEngine.UI;

public class TouchAnimation : MonoBehaviour
{
    [SerializeField] GameObject touchEffectPrefab;
    [SerializeField] Canvas canvas;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Fonctionne aussi pour les écrans tactiles
        {
            Vector2 touchPosition = Input.mousePosition;
            SpawnEffect(touchPosition);
        }
    }

    void SpawnEffect(Vector2 position)
    {
        GameObject effect = Instantiate(touchEffectPrefab, canvas.transform);
        effect.transform.position = position;
        Destroy(effect, 1f); // Détruit l'effet après 1 seconde (modifiable)
    }
}

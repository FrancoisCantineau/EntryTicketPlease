using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class EndMenuAnimation : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // Source audio pour jouer les sons
    [SerializeField] private AudioClip starAppearSound; // Son pour l'apparition des étoiles
    [SerializeField] private AudioClip bigConfettiMusic; // Musique jouée lors de l'apparition des gros confettis

    [SerializeField] private TMP_Text winLoseText;
    [SerializeField] private Transform logo;
    [SerializeField] private Transform[] stars;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float starShakeInterval = 1f;
    [SerializeField] private GameObject confettiFX;
    [SerializeField] private GameObject bigConfettiFX;
    [SerializeField] private Transform bigConfettiSpawnPoint;

    void Start()
    {
        // Laisser les étoiles invisibles au départ
        foreach (var star in stars)
        {
            star.localScale = Vector3.zero;
            star.gameObject.SetActive(true);
        }

        PlayAnimations();
    }

    private void PlayAnimations()
    {
        // Effet d'apparition du texte Win/Lose
        winLoseText.transform.localScale = Vector3.zero;
        winLoseText.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);

        // Effet de zoom et rebond sur le logo
        logo.localScale = Vector3.zero;
        logo.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBounce);

        // Apparition progressive des étoiles avec effets de confettis
        StartCoroutine(AnimateStars());

        // Démarrer l'effet de vague sur le texte Win/Lose après l'animation d'apparition
        StartCoroutine(AnimateWinLoseTextWave());
    }

    private IEnumerator AnimateStars()
    {
        yield return new WaitForSeconds(0.3f); // Laisser un court délai avant l'apparition

        foreach (var star in stars)
        {
            star.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBounce);

            // Jouer le son d'apparition de l'étoile
            PlaySound(starAppearSound);

            SpawnConfetti(star.position); // Effet de confettis à l'apparition de l'étoile
            yield return new WaitForSeconds(0.3f);
        }

        // Lancement du tremblement des étoiles
        StartCoroutine(ShakeStars());

        // Apparition du gros confetti depuis le haut
        yield return new WaitForSeconds(0.5f);
        SpawnBigConfetti();
    }


    private IEnumerator ShakeStars()
    {
        while (true)
        {
            yield return new WaitForSeconds(starShakeInterval);
            foreach (var star in stars)
            {
                star.DOShakeRotation(0.5f, new Vector3(0, 0, 10), 10, 90, false).SetEase(Ease.InOutQuad);
            }
        }
    }

    private IEnumerator AnimateWinLoseTextWave()
    {
        if (winLoseText == null)
        {
            Debug.LogError("WinLoseText n'a pas de composant TextMeshProUGUI !");
            yield break;
        }

        winLoseText.ForceMeshUpdate(); // Mettre à jour le texte pour récupérer les vertices
        TMP_TextInfo textInfo = winLoseText.textInfo;

        float waveSpeed = 5f;  // Vitesse de l'animation
        float waveHeight = 20f; // Amplitude de la vague
        float duration = 200f; // Durée de l'effet de vague
        float time = 0f;

        while (time < duration) // L’animation dure 2 secondes puis s’arrête
        {
            winLoseText.ForceMeshUpdate();
            textInfo = winLoseText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

                // Déplacement sinusoïdal des lettres pour créer une vague
                float yOffset = Mathf.Sin((Time.time * waveSpeed) + (i * 0.3f)) * waveHeight;

                vertices[vertexIndex + 0].y += yOffset;
                vertices[vertexIndex + 1].y += yOffset;
                vertices[vertexIndex + 2].y += yOffset;
                vertices[vertexIndex + 3].y += yOffset;
            }

            // Appliquer les modifications aux vertices
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                winLoseText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            time += Time.deltaTime;
            yield return null;
        }

        // Une fois l'animation terminée, on force la mise à jour du texte pour le stabiliser
        winLoseText.ForceMeshUpdate();
    }


    private void SpawnConfetti(Vector3 position)
    {
        Instantiate(confettiFX, position, Quaternion.identity);
    }

    private void SpawnBigConfetti()
    {
        Instantiate(bigConfettiFX, bigConfettiSpawnPoint.position, Quaternion.identity);

        // Jouer la musique des big confettis
        PlayMusic(bigConfettiMusic);

    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = false; // Désactiver la boucle si on veut la jouer une seule fois
            audioSource.Play();
        }
    }
}
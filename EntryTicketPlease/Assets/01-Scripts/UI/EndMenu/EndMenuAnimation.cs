using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class EndMenuAnimation : MonoBehaviour
{
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
    }

    private IEnumerator AnimateStars()
    {
        yield return new WaitForSeconds(0.3f); // Laisser un court délai avant l'apparition

        foreach (var star in stars)
        {
            star.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBounce);
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

    private void SpawnConfetti(Vector3 position)
    {
        Instantiate(confettiFX, position, Quaternion.identity);
    }

    private void SpawnBigConfetti()
    {
        Instantiate(bigConfettiFX, bigConfettiSpawnPoint.position, Quaternion.identity);
    }
}
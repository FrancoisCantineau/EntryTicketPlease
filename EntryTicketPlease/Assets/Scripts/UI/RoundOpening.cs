using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;
using DG.Tweening;


public class RoundOpening : MonoBehaviour
{
    #region VARIABLES ----------------------------------------------------------------

    [SerializeField] TextMeshProUGUI m_dayNumber;
    [SerializeField] GameObject m_thougtsGrid;
    [SerializeField] GameObject m_thoughtChipPrefab;
    [SerializeField] GameObject m_StartSignal;
    [SerializeField] private Volume globalVolume;
    private DepthOfField depthOfField;

    List<string> thoughts;

    public UnityEvent m_OnOpeningEnd = new UnityEvent();


    #endregion
    #region LIFECYCLE ----------------------------------------------------------------

    private void OnEnable()
    {
        SaveData saveData = SaveManager.Instance.FetchGameData();
        SetDay(saveData.currentDay);
        FetchThoughts();

        // Récupérer la profondeur de champ
        if (globalVolume.profile.TryGet(out depthOfField))
        {
            depthOfField.focusDistance.value = 0f;
        }

        StartCoroutine(PlayOpening());

        // Démarrer l'animation de vague si StartSignal est actif
        if (m_StartSignal.activeInHierarchy)
        {
            StartCoroutine(AnimateStartSignalWave());
        }
    }

    private void OnDisable()
    {
        m_OnOpeningEnd.Invoke();
        m_OnOpeningEnd.RemoveAllListeners();
    }

    #endregion
    #region API ----------------------------------------------------------------------

    #endregion
    #region METHODS ------------------------------------------------------------------

    void FetchThoughts()
    {
        thoughts = new List<string>();
        RoundData roundData = GameManager.CurrentRoundData;
        RoundData defaults = RoundData.Default(roundData.currentDate);

        if (roundData.startingHour < defaults.startingHour)
        {
            thoughts.Add("We had to start early today...");
        }
        else if(roundData.startingHour > defaults.startingHour)
        {
            thoughts.Add("I arrived late today...");
        }

        if(roundData.endingHour < defaults.endingHour)
        {
            thoughts.Add("We'll have to close early...");
        }
        else if (roundData.endingHour > defaults.endingHour)
        {
            thoughts.Add("I have to work overtime today...");
        }

        if(roundData.closedSection != ClosedSection.N)
        {
            thoughts.Add("A section of the park is closed today...");
        }
    }

    void SetDay(int _day)
    {
        m_dayNumber.text = _day.ToString("D2");
    }

    void AddThought(string _thought)
    {
        ThoughtChip thoughtChip = Instantiate(m_thoughtChipPrefab.gameObject, m_thougtsGrid.transform).GetComponent<ThoughtChip>();
        thoughtChip.SetText(_thought);
        thoughtChip.transform.SetParent(m_thougtsGrid.transform);
    }

    IEnumerator PlayOpening()
    {
        var CenterPosition = transform.position;
        var EnterPosition = new Vector3(CenterPosition.x - 1300, CenterPosition.y, CenterPosition.z);
        transform.position = EnterPosition;
        float duration = 1f;
        float time = 0;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(EnterPosition, CenterPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Début de l'effet Depth of Field
        if (depthOfField != null)
        {
            DOTween.To(() => depthOfField.focusDistance.value,
                       x => depthOfField.focusDistance.value = x,
                       0f, 1f);
        }

        time += Time.deltaTime;
        foreach (string thought in thoughts)
        {
            AddThought(thought);
            time += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(6f - time);

        // Réduction progressive du Depth of Field
        if (depthOfField != null)
        {
            DOTween.To(() => depthOfField.focusDistance.value,
                       x => depthOfField.focusDistance.value = x,
                       5f, 1f);
        }

        var ExitPosition = new Vector3(CenterPosition.x + 1300, CenterPosition.y, CenterPosition.z);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(CenterPosition, ExitPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Activer le StartSignal APRÈS les thoughts
        m_StartSignal.SetActive(true);

        // Lancer l'animation une seule fois
        StartCoroutine(AnimateStartSignalWave());

        transform.position = CenterPosition;
    }


    IEnumerator AnimateStartSignalWave()
    {
        TextMeshProUGUI startText = m_StartSignal.GetComponent<TextMeshProUGUI>();

        if (startText == null)
        {
            Debug.LogError("StartSignal n'a pas de composant TextMeshProUGUI !");
            yield break;
        }

        startText.ForceMeshUpdate(); // S'assurer que le texte est bien mis à jour
        TMP_TextInfo textInfo = startText.textInfo;

        float waveSpeed = 5f;  // Vitesse de l'animation
        float waveHeight = 10f; // Amplitude de la vague

        while (m_StartSignal.activeInHierarchy) // Tant que le message est affiché
        {
            startText.ForceMeshUpdate();
            textInfo = startText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

                // Génération d'un effet de vague en modifiant la position verticale
                float yOffset = Mathf.Sin((Time.time * waveSpeed) + (i * 0.3f)) * waveHeight;

                vertices[vertexIndex + 0].y += yOffset;
                vertices[vertexIndex + 1].y += yOffset;
                vertices[vertexIndex + 2].y += yOffset;
                vertices[vertexIndex + 3].y += yOffset;
            }

            // Appliquer les nouvelles positions aux lettres
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                startText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null; // Attendre la prochaine frame pour mettre à jour l'animation
        }
    }

    #endregion
    #region EVENTS -------------------------------------------------------------------

    #endregion
}




using UnityEngine;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private bool isSoundOn;

    void Start()
    {
        // Charger l'état du son
        isSoundOn = PlayerPrefs.GetInt("SoundState", 1) == 1;
        UpdateSoundState();
        soundButton.onClick.AddListener(ToggleAudio);
    }

    private void ToggleAudio()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundState", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSoundState();
    }

    private void UpdateSoundState()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
        soundButton.image.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }
}

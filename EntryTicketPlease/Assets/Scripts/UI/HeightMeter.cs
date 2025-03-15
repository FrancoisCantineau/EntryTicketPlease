using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightMeter : MonoBehaviour
{
    [SerializeField] Slider m_meter;

    [SerializeField] int visitorMinHeight = 100;
    [SerializeField] int visitorMaxHeight = 220;

    void Start()
    {
        
    }

    public void UpdateMeterLimit(float currentHeight, bool shouldMesure)
    {
        Debug.Log("renre");
        if (shouldMesure) 
        { 
            m_meter.gameObject.SetActive(true);
            float normalizedValue = (float)(currentHeight - visitorMinHeight) / (visitorMaxHeight - visitorMinHeight);
            m_meter.value = Mathf.Clamp01(normalizedValue);
        }
        else 
        {
            Debug.Log("ffg");
            m_meter.gameObject.SetActive(false);
        }
        
    }
}

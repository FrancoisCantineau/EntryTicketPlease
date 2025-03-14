using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    [SerializeField] GameObject HUD;
    private ClockScript clock;
    private int ratio;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(50,-30,0);
        clock = HUD.GetComponent<ClockScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ratio = 140 / clock.durationReturn;
        gameObject.transform.Rotate( ratio * Time.deltaTime,0,0);
        print(clock.durationReturn);
    }
}

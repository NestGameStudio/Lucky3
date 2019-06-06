using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public float SlideLastValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<SceneController>().fadeOut == false)
        {
            mixer.SetFloat("Volume", slider.value);
        }
    }
    public void LastChanged()
    {
        SlideLastValue = slider.value;
    } 
}

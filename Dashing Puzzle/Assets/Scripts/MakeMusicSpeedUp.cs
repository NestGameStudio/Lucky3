using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MakeMusicSpeedUp : MonoBehaviour
{
    private float timer = 0.0f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        gameObject.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Pitch",minPitch+(timer/280));
    }
}

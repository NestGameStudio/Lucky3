using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomByTime : MonoBehaviour
{
    float ZoomRatio;
    public GameObject timeScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ZoomRatio = timeScript.GetComponent<TimeRushController>().currentTime;
        //Debug.Log(ZoomRatio);
        float velocity = 0;
        //velocity += 1f;
        float normalizedValue = Mathf.InverseLerp(0, 60, ZoomRatio);
        float result = Mathf.Lerp(6, 4f, normalizedValue);

        gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = result;
        
    }
}

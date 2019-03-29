﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunset : MonoBehaviour
{
    public float timeOffset; // amount of time, in seconds, to offset from sunrise
    public float time;  
    public Transform sun;    // directional lights transform
    public Light sunLight;  // sunlight
    public float intensity; // how intense the light is
    public Color fogday = Color.grey;  // color of fog during the day
    public Color fognight = Color.black;  // color of fog at night

    public int speed;   // speed of sunset 
    public float ambientIntensity;  // intensity of ambient light
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sunset());
    }

    IEnumerator sunset()
    {

        bool isDay = true;
        while (isDay)
        {
            time += Time.deltaTime * speed;
            float offsetTime = time + timeOffset;

            // set up variables for controlling ambient light during sunset
            float sunsetStart = 54000;
            float sunsetEnd = 58100;
            float totalNight = 60000;
            float scaleDownFactor = 5000;

            if(offsetTime >= sunsetStart && offsetTime <= sunsetEnd) {
                // scale down ambient lighting during sunset
                ambientIntensity = (sunsetEnd - offsetTime) / scaleDownFactor;
                RenderSettings.reflectionIntensity = ambientIntensity;
                RenderSettings.ambientIntensity = ambientIntensity;
            }else if(offsetTime >= sunsetEnd) {
                // turn off ambient lighting
                RenderSettings.reflectionIntensity = 0;
                RenderSettings.ambientIntensity = 0;
            }

            // set up variables for controlling sun rotation/fog settings
            float secondsInADay = 60 * 60 * 24;
            float halfDay = secondsInADay / 2;
            float quarterDay = halfDay / 2;

            // rotate sun and calculate intensity
            sun.rotation = Quaternion.Euler(new Vector3((offsetTime - quarterDay) / 64000 * 360, 0, 0));
            intensity = 1 - ((halfDay - offsetTime) / halfDay * -1);

            // set fog color
            RenderSettings.fogColor = Color.Lerp(fognight, fogday, intensity * intensity);
            sunLight.intensity = intensity;

            // turn off sun, exit coroutine
            if(offsetTime >= totalNight)
            {
                sunLight.gameObject.SetActive(false);
                isDay = false;
            }

            yield return null;
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

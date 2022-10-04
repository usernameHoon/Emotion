using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CafofoStudio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    CaveAmbientMixer caveMixer;
    

    // 冠零 家府 眉农
    public delegate void batSoundHandler();
    public event batSoundHandler BatSoundCheck;
    [HideInInspector] public int batSoundCnt;

    // 拱家府 眉农
    public delegate void waterSoundHandler();
    public event waterSoundHandler waterSoundCheck;
    [HideInInspector] public int waterSoundCnt;

    void Awake() {
        instance = this;

        caveMixer = GameObject.Find("CaveAmbience") ? .GetComponent<CaveAmbientMixer>();
    }

    void Update()
    {
        BatSoundCheckUpdate();
        WaterSoundCheckUpdate();
    }
    void BatSoundCheckUpdate() {
        batSoundCnt = 0;
        BatSoundCheck();
        if (batSoundCnt > 0 && caveMixer.Critters.GetIntensity() != 1) {
            caveMixer.Critters.SetIntensity(1);
        }
        else if(batSoundCnt <= 0 && caveMixer.Critters.GetIntensity() != 0) {
            caveMixer.Critters.SetIntensity(0);
        }
    }
    void WaterSoundCheckUpdate() {
        waterSoundCnt = 0;
        waterSoundCheck();
        if(waterSoundCnt > 0 && caveMixer.WaterStream.GetIntensity() != 1) {
            caveMixer.WaterStream.SetIntensity(1);
        }
        else if(waterSoundCnt <= 0 && caveMixer.WaterStream.GetIntensity() != 0) {
            caveMixer.WaterStream.SetIntensity(0);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Parameters")] 
    public float musicVolume;
    public float sfxVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}

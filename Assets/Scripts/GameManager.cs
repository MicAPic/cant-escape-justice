using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwipeableView;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")] 
    public TMP_Text caseCounter;
    [SerializeField] 
    private UISwipeableViewCourtroom swipeableView;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        var data = Enumerable.Range(0, 1000)
            .Select(i => new DefendantRecord
            {
                color = new Color(Random.value, Random.value, Random.value, 1.0f),
                isGuilty = Random.value > 0.5f // random bool
            })
            .ToList();

        swipeableView.UpdateData(data);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
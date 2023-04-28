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
    
    [Header("Generator")]
    public StringsRandomData charges;
    public StringsRandomData schedules;
    public StringsRandomData timesOfCrime;
    // public StringsRandomData scheduleTime;
    // public ImageRandomData items; 
    
    [Header("UI")]
    public GameObject[] cases;
    public TMP_Text[] caseCounters;
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

        var dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager)
        {
            dialogueManager.StartDialogue();    
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var data = Enumerable.Range(0, 1000)
            .Select(i => new DefendantRecord
            {
                color = new Color(Random.value, Random.value, Random.value, 1.0f),
                isGuilty = Random.value > 0.5f // random bool
                // charge = ...
                // schedule = ...
                // items = ...
            })
            .ToList();

        swipeableView.UpdateData(data);
    }

    public void SwitchCases()
    {
        cases[0].transform.SetSiblingIndex(0);
        (cases[0], cases[1]) = (cases[1], cases[0]);
        (caseCounters[0], caseCounters[1]) = (caseCounters[1], caseCounters[0]);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
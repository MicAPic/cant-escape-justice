using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SwipeableView;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Rules")]

    [Header("Generator")]
    public StringsRandomData charges;
    public StringsRandomData schedules;
    public StringsRandomData timesOfCrime;
    public StringsRandomData scheduleTime;
    public ImageRandomData hairData;
    public ImageRandomData eyeData;
    public ImageRandomData mouthData;
    public ImageRandomData items; 
    
    [Header("UI")]
    public CanvasGroup raycastBlock;
    public GameObject[] cases;
    public TMP_Text[] caseCounters;
    public TMP_Text[] caseSchedules;
    public TMP_Text[] caseDescriptions;
    [SerializeField] 
    private UISwipeableViewCourtroom swipeableView;

    [Header("Dialogue")] 
    private DialogueManager _dialogueManager;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _dialogueManager = FindObjectOfType<DialogueManager>();
        if (_dialogueManager)
        {
            _dialogueManager.StartDialogue();    
        }
    }

    private bool curGuilty;
    private string curTimeOfCrime;
    // Start is called before the first frame update
    void Start()
    {
        List<DefendantRecord> data;
        
        if (_dialogueManager)
        {
            data = Enumerable.Range(0, 1000)
                .Select(i => new DefendantRecord
                {
                    color = i % 2 == 0 ? _dialogueManager.gavelColor : _dialogueManager.defendantColor,
                    isGuilty = false,
                    charge = string.Empty,
                    timeOfCrime = string.Empty,
                    //weapon = items.images[(int)(Random.value * 100) % items.images.Count]

                    schedule = string.Empty

                    //items = ...

                    //face = ...
                })
                .ToList();

            data[1].color = _dialogueManager.courtroomColor;
            data[0].color = _dialogueManager.defendantColor;
        }
        else
        {
            data = Enumerable.Range(0, 1000)
                .Select(i => new DefendantRecord
                {
                    color = new Color(Random.value, Random.value, Random.value, 1.0f),
                    isGuilty = GenerateGuilty(), // random bool
                    charge = GenerateCharge(),
                    timeOfCrime = GenerateTimeOfCrime(),
                    //weapon = items.images[(int)(Random.value * 100) % items.images.Count]

                    schedule = GenerateSchedule(curGuilty, curTimeOfCrime)

                    //items = ...

                    //face = ...
                })
                .ToList();
        }

        swipeableView.UpdateData(data);
    }
    private bool GenerateGuilty()
    {
        curGuilty = Random.value > 0.5f;
        return curGuilty;
    }
    private string GenerateTimeOfCrime()
    {
        curTimeOfCrime = timesOfCrime.strings[(int)(Random.value * 100) % timesOfCrime.strings.Count];
        return curTimeOfCrime;
    }

    private string GenerateCharge()
    {
        return charges.strings[((int)(Random.value * 100)) % charges.strings.Count];
    }
    
    private string GenerateSchedule(bool isGuilty, string timeOfCrime)
    {
        StringBuilder schedule = new StringBuilder(100);
        int firstScheduleH;
        int secondScheduleH;
        int hours = int.Parse(timeOfCrime.Split(":")[0]);
        if (!isGuilty)
        {
            switch (hours)
            {
                case < 13:
                    firstScheduleH = 8 + (int)(Random.value * 100) % (hours - 7);
                    secondScheduleH = hours + 1 + (int)(Random.value * 100) % (13 - hours);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 13 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (18 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 18 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (23 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
                case < 18:
                    firstScheduleH = 8 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (13 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 13 + (int)(Random.value * 100) % (hours - 12);
                    secondScheduleH = hours + 1 + (int)(Random.value * 100) % (18 - hours);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 18 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (23 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
                default:
                    firstScheduleH = 8 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (13 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 13 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (18 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 18 + (int)(Random.value * 100) % (hours - 17);
                    secondScheduleH = hours + 1 + (int)(Random.value * 100) % (23 - hours);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
            }
        }
        else
        {
            switch (hours)
            {
                case < 13:
                    switch (hours)
                    {
                        case 8:
                            firstScheduleH = 10;
                            secondScheduleH = 12;
                            break;
                        case 12:
                            firstScheduleH = 10;
                            secondScheduleH = 11;
                            break;
                        default:
                            firstScheduleH = hours - 1;
                            secondScheduleH = hours;
                            break;
                    }
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 13 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (18 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 18 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (23 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
                case < 18:
                    firstScheduleH = 8 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (13 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    switch (hours)
                    {
                        case 13:
                            firstScheduleH = 15;
                            secondScheduleH = 16;
                            break;
                        case 17:
                            firstScheduleH = 14;
                            secondScheduleH = 15;
                            break;
                        default:
                            firstScheduleH = hours - 1;
                            secondScheduleH = hours;
                            break;
                    }
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 18 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (23 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
                default:
                    firstScheduleH = 8 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (13 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    firstScheduleH = 13 + (int)(Random.value * 100) % 5;
                    secondScheduleH = firstScheduleH + 1 + (int)(Random.value * 100) % (18 - firstScheduleH);
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>\n");

                    switch (hours)
                    {
                        case 18:
                            firstScheduleH = 21;
                            secondScheduleH = 22;
                            break;
                        case 23:
                            firstScheduleH = 20;
                            secondScheduleH = 22;
                            break;
                        default:
                            firstScheduleH = hours - 1;
                            secondScheduleH = hours;
                            break;
                    }
                    schedule.Append($"{firstScheduleH:00}:00–{secondScheduleH:00}:00 <indent=55%>{GenerateScheduleBusiness()}</indent>");
                    break;
            }
        }
        return schedule.ToString();
    }

    private string GenerateScheduleBusiness()
    {
        return schedules.strings[(int)(Random.value * 100) % schedules.strings.Count];
    }

    public void SwitchCases()
    {
        cases[0].transform.SetSiblingIndex(0);
        (cases[0], cases[1]) = (cases[1], cases[0]);
        (caseCounters[0], caseCounters[1]) = (caseCounters[1], caseCounters[0]);
        (caseDescriptions[0], caseDescriptions[1]) = (caseDescriptions[1], caseDescriptions[0]);
        (caseSchedules[0], caseSchedules[1]) = (caseSchedules[1], caseSchedules[0]);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
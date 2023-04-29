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
    public GameObject[] cases;
    public TMP_Text[] caseCounters;
    public TMP_Text[] caseSchedules;
    public TMP_Text[] caseDescriptions;
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


    private bool curGuilty;
    private string curTimeOfCrime;
    private ProofOfInnocence curProof;
    private enum ProofOfInnocence
    {
        SCHEDUAL,
        FACE,
        ITEMS
    }
    // Start is called before the first frame update
    void Start()
    {
        var data = Enumerable.Range(0, 1000)
            .Select(i => new DefendantRecord
            {
                color = new Color(Random.value, Random.value, Random.value, 1.0f),
                isGuilty = GenerateGuilty(), // random bool
                charge = GenerateCharge(),// charge
                timeOfCrime = GenerateTimeOfCrime(),
                //weapon = items.images[(int)(Random.value * 100) % items.images.Count]
                
                schedule = GenerateSchedule(curGuilty, curTimeOfCrime)

                //items = ...

                //face = ...
            })
            .ToList();

        swipeableView.UpdateData(data);
    }
    private bool GenerateGuilty()
    {
        curGuilty = Random.value > 0.5f;
        //if (curGuilty)
        //{
        //    curProof = (ProofOfInnocence)((int)(Random.value * 100) % 3);
        //}
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
        StringBuilder schedual = new StringBuilder(100);
        int firstSchedualH;
        int secondSchedualH;
        if (!isGuilty /*|| curProof != ProofOfInnocence.SCHEDUAL*/)
        {
            int hours = int.Parse(timeOfCrime.Split(":")[0]);
            //int minutes = int.Parse(timeOfCrime.Split(":")[1]);
            
            if (hours < 13)
            {
                firstSchedualH = 8 + (int)(Random.value * 100) % (hours - 7);
                secondSchedualH = hours + 1 + (int)(Random.value * 100) % (13 - hours);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 13 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (18 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 18 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (23 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>");
            } 
            else if(hours >= 13 && hours < 18)
            {
                firstSchedualH = 8 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (13 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 13 + (int)(Random.value * 100) % (hours - 12);
                secondSchedualH = hours + 1 + (int)(Random.value * 100) % (18 - hours);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 18 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (23 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>");
            }
            else
            {
                firstSchedualH = 8 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (13 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 13 + (int)(Random.value * 100) % 5;
                secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (18 - firstSchedualH);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

                firstSchedualH = 18 + (int)(Random.value * 100) % (hours - 17);
                secondSchedualH = hours + 1 + (int)(Random.value * 100) % (23 - hours);
                schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>");

            }
        }
        else
        {
            firstSchedualH = 8 + (int)(Random.value * 100) % 5;
            secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (13 - firstSchedualH);
            schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

            firstSchedualH = 13 + (int)(Random.value * 100) % 5;
            secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (18 - firstSchedualH);
            schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>\n");

            firstSchedualH = 18 + (int)(Random.value * 100) % 5;
            secondSchedualH = firstSchedualH + 1 + (int)(Random.value * 100) % (23 - firstSchedualH);
            schedual.Append($"{firstSchedualH}:00本secondSchedualH}:00 <indent=55%>{GenerateSchedualBuisness()}</indent>");
        }
        return schedual.ToString();
    }

    private string GenerateSchedualBuisness()
    {
        return schedules.strings[((int)(Random.value * 100)) % schedules.strings.Count];
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
using System.Collections.Generic;
using System.Linq;
using SwipeableView;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Animation")] 
    [SerializeField]
    private Vector2 caseSlidePivot = new(500, 0); 
    [SerializeField] 
    private float caseSlideAngle = 45f;
    [SerializeField]
    private Vector2 swipeableViewSlidePivot;
    [SerializeField] 
    private float swipeableViewSlideAngle;
    [SerializeField] 
    private float swipeableViewSlideDuration = 1.0f;
    
    [Header("Generator")]
    public float epsilon = 0.7f;
    public StringsRandomData charges;
    public StringsRandomData schedules;
    
    public ImageRandomData bodyData;
    public ImageRandomData hairData;
    public ImageRandomData eyeData;
    public ImageRandomData mouthData;
    private List<Sprite> _allFeatures;

    public ImageRandomData itemData;
    private List<Sprite> _allItems;
    
    [Header("UI")]
    public CanvasGroup raycastBlock;
    public GameObject[] cases;
    public TMP_Text[] caseCounters;
    public TMP_Text[] caseSchedules;
    public TMP_Text[] caseDescriptions;
    public Image[] caseAItems;
    public Image[] caseBItems;
    [SerializeField] 
    private UISwipeableViewCourtroom swipeableView;
    [SerializeField] 
    private TMP_Text score;

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

        _allFeatures = hairData.images.Concat(eyeData.images.Concat(mouthData.images)).ToList();
    }
    
    private bool curGuilty;
    private string curTimeOfCrime;

    private List<string> _curFeatures;
    private List<string> _curItems;
    // Start is called before the first frame update
    void Start()
    {
        List<DefendantRecord> data;
        
        if (_dialogueManager)
        {
            data = Enumerable.Range(0, 1000)
                .Select(i => new DefendantRecord
                {
                    potato = i % 2 == 0 ? _dialogueManager.gavelSprite : _dialogueManager.defendantSprite,
                    eyes = null,
                    mouth = null,
                    hair = null,
                    
                    isGuilty = false,
                    charge = string.Empty,
                    timeOfCrime = string.Empty,
                    weapon = null,

                    schedule = string.Empty,

                    items = null
                })
                .ToList();

            data[1].potato = _dialogueManager.courtroomSprite;
            data[0].potato = _dialogueManager.defendantSprite;
        }
        else
        {
            data = Enumerable.Range(0, 1000)
                .Select(_ => new DefendantRecord
                {
                    potato = GenerateBody(),
                    eyes = GenerateFeature(eyeData),
                    mouth = GenerateFeature(mouthData),
                    hair = GenerateFeature(hairData),
                    
                    items = GenerateItemsList(),

                    isGuilty = GenerateGuilt(),
                    charge = GenerateCharge(),
                    timeOfCrime = GenerateTimeOfCrime(),

                    feature = GenerateIncriminatingFeature(),
                    weapon = GenerateIncriminatingItems(),

                    schedule = GenerateSchedule(curGuilty, curTimeOfCrime)
                })
                .ToList();
        }

        swipeableView.UpdateData(data);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            UITransitionController.Instance.TransitionAndLoad(SceneManager.GetActiveScene().name);
        }
    }
    
    private Sprite GenerateBody()
    {
        _curFeatures = new List<string>();
        return bodyData.images[Random.Range(0, bodyData.images.Count)];
    }
    private Sprite GenerateFeature(ImageRandomData featureData)
    {
        var feature = featureData.images[Random.Range(0, featureData.images.Count)]; 
        _curFeatures.Add(feature.name);
        return feature;
    }
    private List<Sprite> GenerateItemsList()
    {
        _curItems = new List<string>();
        List<Sprite> res = new List<Sprite>();
        for(int i = 0; i < 5; ++i)
        {
            var item = itemData.images[Random.Range(0, itemData.images.Count)];
            res.Add(item);
            _curItems.Add(item.name);
        }
        return res;
    }
    private bool GenerateGuilt()
    {
        // random bool:
        curGuilty = Random.value > 0.5f;
        return curGuilty;
    }
    private string GenerateTimeOfCrime()
    {
        int hours = Random.Range(8, 23);
        int minutes = Random.Range(1, 59);
        curTimeOfCrime = $"{hours:00}:{minutes:00}";

        return curTimeOfCrime;
    }
    private string GenerateCharge()
    {
        return charges.strings[(int)(Random.value * 100) % charges.strings.Count];
    }
    private string GenerateIncriminatingFeature()
    {
        var probability = curGuilty ? 1.0f : Random.value;

        return probability > epsilon ? 
            _curFeatures[Random.Range(0, _curFeatures.Count)] : 
            _allFeatures[Random.Range(0, _allFeatures.Count)].name;
    }
    private string GenerateIncriminatingItems()
    {
        var probability = curGuilty ? 1.0f : Random.value;

        return probability > epsilon ?
            _curItems[Random.Range(0, _curItems.Count)] :
            itemData.images[Random.Range(0, itemData.images.Count)].name;
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

    public void GameOver()
    {
        score.text = $"High score: {SettingsManager.Instance.highScore}";
        Timer.Instance.stopTimer = true;
        
        var rect = swipeableView.transform/*.parent*/.GetComponent<RectTransform>();
        rect.DOShapeCircle(swipeableViewSlidePivot, swipeableViewSlideAngle, swipeableViewSlideDuration);
    }

    public void SwitchCases()
    {
        (cases[0], cases[1]) = (cases[1], cases[0]);
        (caseCounters[0], caseCounters[1]) = (caseCounters[1], caseCounters[0]);
        (caseDescriptions[0], caseDescriptions[1]) = (caseDescriptions[1], caseDescriptions[0]);
        (caseSchedules[0], caseSchedules[1]) = (caseSchedules[1], caseSchedules[0]);
        (caseAItems, caseBItems) = (caseBItems, caseAItems);

        var rect = cases[1].GetComponent<RectTransform>();
        var defaultPos = rect.anchoredPosition;
        
        rect.DOShapeCircle(caseSlidePivot, caseSlideAngle, 1.0f)
            .OnComplete(() => ResetCasePos(rect, defaultPos));
    }

    private void ResetCasePos(RectTransform rect, Vector2 defaultPos)
    {
        rect.SetSiblingIndex(0);
        rect.anchoredPosition = defaultPos;
    }
}
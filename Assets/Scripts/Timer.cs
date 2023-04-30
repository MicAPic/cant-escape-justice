using System.Collections;
using System.Collections.Generic;
using SwipeableView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    
    public float gameTime = 5.0f;
    public float cooldown = 1.0f;
    public bool stopTimer;
    
    private Slider _timerSlider;
    // [SerializeField]
    // private TMP_Text timerText;
    private float _spendTime;
    private bool _wasFrozen;
    private DialogueManager _dialogueManager;

    public AnimationCurve animCurve;
    private int _currentCard;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _timerSlider = GetComponent<Slider>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameTime = animCurve.Evaluate(_currentCard);
        _timerSlider.maxValue = gameTime;
        _timerSlider.value = gameTime;
        // timerText.text = $"00:{gameTime:00}";
        _currentCard = 0;
        StartCoroutine(FreezeForAMomentAndUpdate(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer) return;
        float time = gameTime - _spendTime;

        // int seconds = (int)time + 1;

        // string textTime = $"00:{seconds:00}";

        if (time <= 0)
        {
            stopTimer = true;
            GameManager.Instance.raycastBlock.blocksRaycasts = false;
            
            if (_dialogueManager)
            {
                stopTimer = true;

                _dialogueManager.SelectChoice(0);
                
                var currentCard = GameManager.Instance.raycastBlock
                    .GetComponentsInChildren<UISwipeableCardCourtroom>()[^1];
                currentCard.AutoSwipeLeft(currentCard.cachedRect.localPosition);
                GameManager.Instance.SwitchCases();
                
                return;
            }
            
            GameManager.Instance.GameOver();
        }
        
        if (_wasFrozen)
        {
            _spendTime += Time.deltaTime;
            // timerText.text = textTime;
            _timerSlider.value = time;
        }
        // else
        // {
        //     // timerText.text = "00:00";
        //     timerSlider.value = 0;
        // }
    }

    public void Reset()
    {
        ++_currentCard;
        _spendTime = 0.0f;
        gameTime = animCurve.Evaluate(_currentCard);
        _timerSlider.maxValue = gameTime;
        _timerSlider.value = gameTime;
        _wasFrozen = false;
        StartCoroutine(FreezeForAMomentAndUpdate(cooldown));
    }

    private IEnumerator FreezeForAMomentAndUpdate(float time)
    {
        yield return new WaitForSeconds(time);

        _wasFrozen = true;
    }
}

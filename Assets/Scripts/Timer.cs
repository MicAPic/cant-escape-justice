using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Slider timerSlider;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private float gameTime;

    private float spendTime = 0;

    public bool stopTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
        timerText.text = $"00:{gameTime:00}";
        StartCoroutine(FreezeForAMomentAndUpdate());
    }

    // Update is called once per frame
    bool wasFreze = false;
    void Update()
    {
        if (!wasFreze) return;
        spendTime += Time.deltaTime;
        float time = gameTime - spendTime;

        int seconds = (int)time + 1;

        string textTime = $"00:{seconds:00}";

        if (time <= 0)
        {
            stopTimer = true;
        }

        if (stopTimer == false)
        {
            timerText.text = textTime;
            timerSlider.value = time;
        }
        else
        {
            timerText.text = "00:00";
            timerSlider.value = 0;
        }
    }

    private IEnumerator FreezeForAMomentAndUpdate()
    {
        yield return new WaitForSeconds(1);

        wasFreze = true;
    }
}

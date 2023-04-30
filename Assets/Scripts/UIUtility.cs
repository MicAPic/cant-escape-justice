using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIUtility : MonoBehaviour
{
    [SerializeField] 
    private RectTransform menuGroupRect;
    [SerializeField] 
    private float settingsTransitionDuration;
    [SerializeField] 
    private Slider musicSlider;
    [SerializeField] 
    private Slider sfxSlider;
    private float _defaultPosX;
    
    // // Start is called before the first frame update
    void Start()
    {
        _defaultPosX = menuGroupRect.anchoredPosition.x;
        musicSlider.value = SettingsManager.Instance.musicVolume;
        sfxSlider.value = SettingsManager.Instance.sfxVolume;
    }
    
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public void MoveMenuGroup()
    {
        var pos = menuGroupRect.anchoredPosition;
        if (Mathf.Abs(_defaultPosX - pos.x) < Mathf.Abs(-_defaultPosX - pos.x))
        {
            menuGroupRect.DOAnchorPosX(-_defaultPosX, settingsTransitionDuration);
        }
        else
        {
            menuGroupRect.DOAnchorPosX(_defaultPosX, settingsTransitionDuration);
        }
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}

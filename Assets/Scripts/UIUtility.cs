using Audio;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIUtility : MonoBehaviour
{
    public bool isMainMenu;
    
    [SerializeField] 
    private RectTransform menuGroupRect;
    [SerializeField] 
    private CanvasGroup clickInfoGroup;
    [SerializeField] 
    private float hideDuration;
    [SerializeField] 
    private float settingsTransitionDuration;
    [SerializeField] 
    private Slider musicSlider;
    [SerializeField] 
    private Slider sfxSlider;
    [SerializeField] 
    private AudioClip sfxSound;
    private float _defaultPosX;
    
    // // Start is called before the first frame update
    void Start()
    {
        if (!isMainMenu) return;
        
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

    public void PlaySfx()
    {
        var sfxPlayer = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
        sfxPlayer.PlayOneShot(sfxSound);
    }

    public void HideInfoGroup()
    {
        clickInfoGroup.DOFade(0.0f, hideDuration);
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

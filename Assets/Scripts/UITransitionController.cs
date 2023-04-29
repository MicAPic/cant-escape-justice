using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITransitionController : MonoBehaviour
{
    public static UITransitionController Instance;
    [SerializeField] 
    private float duration = 1.0f;
    private UITransitionEffect _transitionEffect;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        _transitionEffect = GetComponent<UITransitionEffect>();
        _transitionEffect.effectFactor = 1.0f;
        _transitionEffect.effectPlayer.duration = duration;
    }

    // Start is called before the first frame update
    void Start()
    {
        _transitionEffect.Hide();
    }

    public void TransitionAndLoad(string sceneName)
    {
        DOTween.To(
            () => _transitionEffect.effectFactor, 
            x => _transitionEffect.effectFactor = x, 
            1.0f, 
            duration
                   ).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}

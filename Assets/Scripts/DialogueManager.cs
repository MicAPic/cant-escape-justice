using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ink.Runtime;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Parameters")] 
    [SerializeField]
    private float textSpeed = 0.04f;
    [SerializeField]
    private float skipCooldownTime = 0.5f;

    [Header("UI")]
    [SerializeField] 
    private TMP_Text speakerText;
    [SerializeField] 
    private TMP_Text dialogueText;
    [SerializeField] 
    private GameObject continueIcon;

    [Header("Ink")]
    [SerializeField] 
    private TextAsset inkScript;
    private Story _story;
    private const string SpeakerTag = "speaker";

    [Header("Audio")] 
    [SerializeField] 
    private DialogueAudioInfo defaultAudioInfo;
    [SerializeField] 
    private DialogueAudioInfo[] audioInfos;
    private DialogueAudioInfo _currentAudioInfo;
    private Dictionary<string, DialogueAudioInfo> audioInfoConfigurations;
    [Range(1, 5)]
    [SerializeField] 
    private int frequencyLevel = 2;
    private AudioSource _audioSource;
    [SerializeField]
    private bool stopAudioSource;
    
    private bool _isPlaying;
    private bool _isDisplayingRichText;
    private int _maxLineLength;
    private bool _canContinue;
    private bool _canSkip;
    private Coroutine _displayLineCoroutine;
    private Coroutine _skipCooldownCoroutine;
    
    void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _audioSource = gameObject.AddComponent<AudioSource>();
        InitializeAudioInfoDictionary();
        _currentAudioInfo = defaultAudioInfo;
    }

    void Update()
    {
        if (!_isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_canContinue)
            {
                ContinueStory();   
            }
            else if (_canSkip && _story.canContinue)
            {
                StopCoroutine(_displayLineCoroutine);
                dialogueText.maxVisibleCharacters = _maxLineLength;
                FinishDisplayingLine();
            }
        }
    }
    
    public void StartDialogue()
    {

        _story = new Story(inkScript.text);
        // _story.BindExternalFunction("finishLevel", (string mode) =>
        // {
        //     
        // });

        StartCoroutine(WaitBeforeDisplayingText());
    }
    
    private IEnumerator WaitBeforeDisplayingText()
    {
        yield return new WaitForSeconds(0.66f);
        ContinueStory();
        _isPlaying = true;
    }

    private void ContinueStory()
    {
        if (_story.canContinue)
        {
            // ClearChoices();
            if (_skipCooldownCoroutine != null)
            {
                StopCoroutine(_skipCooldownCoroutine);
            }

            string nextLine = _story.Continue();
            HandleTags(_story.currentTags);
            _displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            _isPlaying = false;
            // StartCoroutine(WaitBeforeGivingControl());
        }
    }

    // private IEnumerator WaitBeforeGivingControl()
    // {
    //     yield return new WaitForSeconds(0.1f);
    // }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = line;
        _maxLineLength = line.Length;

        continueIcon.SetActive(false);
        _canContinue = false;
        _canSkip = false;

        _skipCooldownCoroutine = StartCoroutine(SkipCooldown());
        for (var i = 0; i < _maxLineLength; i++)
        {
            // rich text
            switch (dialogueText.text[i])
            {
                case '<':
                    _isDisplayingRichText = true;
                    break;
                case '>':
                    _isDisplayingRichText = false;
                    _maxLineLength--;
                    break;
            }
        
            if (_isDisplayingRichText)
            {
                _maxLineLength--;
            }
            //
            PlayDialogueSound(i, dialogueText.text[i]);
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }

        FinishDisplayingLine();
    }

    private IEnumerator SkipCooldown()
    {
        yield return new WaitForSeconds(skipCooldownTime);
        _canSkip = true;
    }

    private void FinishDisplayingLine()
    {
        _isDisplayingRichText = false;
        continueIcon.SetActive(true);
        _canContinue = true;
        _canSkip = true;
    }

    private void PlayDialogueSound(int currentLineLength, char currentCharacter)
    {
        if (currentLineLength % frequencyLevel != 0) return;
        
        var typingAudioClips = _currentAudioInfo.typingAudioClips;
        var minPitch = _currentAudioInfo.minPitch;
        var maxPitch = _currentAudioInfo.maxPitch;

        if (stopAudioSource)
        {
            _audioSource.Stop();
        }

        // clip
        var characterHash = currentCharacter.GetHashCode();
        var audioClip = typingAudioClips[characterHash % typingAudioClips.Length];
        
        // pitch
        var maxPitchInt = (int) maxPitch * 100;
        var minPitchInt = (int) minPitch * 100;
        var pitchRange = maxPitchInt - minPitchInt;
        if (pitchRange != 0)
        {
            _audioSource.pitch = (characterHash % pitchRange + minPitchInt) / 100f; 
        }
        else
        {
            _audioSource.pitch = minPitch;
        }
        
        _audioSource.PlayOneShot(audioClip);
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (var tag in currentTags)
        {
            string[] pair = tag.Split(':');
            if (pair.Length != 2)
            {
                Debug.LogError("Tag couldn't be parsed:" + tag);
            }

            string key = pair[0];
            string value = pair[1];

            switch (key)
            {
                case SpeakerTag:
                    speakerText.text = value;
                    SetCurrentAudioInfo(value);
                    break;
                default:
                    Debug.LogWarning("Given tag is not implemented:" + key);
                    break;
            }
        }
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoConfigurations = new Dictionary<string, DialogueAudioInfo> {{defaultAudioInfo.id, defaultAudioInfo}};
        foreach (var audioInfo in audioInfos)
        {
            audioInfoConfigurations.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        audioInfoConfigurations.TryGetValue(id, out var audioInfo);

        if (audioInfo != null)
        {
            _currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio for id: " + id);
        }
    }
}
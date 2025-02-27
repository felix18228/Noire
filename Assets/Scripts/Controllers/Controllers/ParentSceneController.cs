﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
/// A parent scene controller supports virtual implementations of the following functions:
/// <code>Init</code> Called in Awake()
/// <code>LateInit</code> Called in Start(), This is called after DataPersistence!
/// </summary>

public class ParentSceneController : MonoBehaviour
{
    [Header("Interactable Objects")]
    [SerializeField] protected InteractableObject[] unaffectedInteractableObjects;
    
    [Header("Audio")]
    [SerializeField] protected FMODUnity.EventReference bgmAudioEvent;
    
    [Header("Title text")]
    [SerializeField] private CanvasGroup sceneTitle;
    [SerializeField] private CanvasGroup UI;
    [SerializeField] private AnimationCurve titleIntensityCurve;
    [SerializeField] private AnimationCurve UIIntensityCurve;
    private float titleAnimationTime = 3;
    
    protected List<InteractableObject> interactablesList;

    private void Awake()
    {
        SceneManager.sceneLoaded += FindAllInteractables;
        Init();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FindAllInteractables;
    }

    private void Start()
    {
        foreach (var obj in unaffectedInteractableObjects)
            obj.Enable();
        StartCoroutine(DisplaySceneName());
        LateInit();
    }

    protected virtual void Init() { }

    protected virtual void LateInit() { }

    private IEnumerator DisplaySceneName()
    {
        GameEventsManager.Instance.GameStateEvents.MenuToggle(true);
        
        sceneTitle.gameObject.SetActive(false);
        UI.alpha = 0;
        yield return new WaitForSeconds(1);
        
        AudioManager.Instance.PlaySceneBegins();
        
        sceneTitle.gameObject.SetActive(true);
        float time = 0;
        while (time < 1)
        {
            sceneTitle.alpha = Mathf.Lerp(1, 0, titleIntensityCurve.Evaluate(time));
            time += Time.deltaTime / titleAnimationTime;
            yield return null;
        }
        sceneTitle.gameObject.SetActive(false);
        
        time = 0;
        while (time < 1)
        {
            UI.alpha = Mathf.Lerp(0, 1, UIIntensityCurve.Evaluate(time));
            time += Time.deltaTime;
            yield return null;
        }
        UI.alpha = 1;
        
        GameEventsManager.Instance.GameStateEvents.MenuToggle(false);
    }
    
    protected void ToggleAllInteractables(bool active)
    {
        if (active)
            foreach (var interactable in interactablesList)
                interactable.Enable();
        else
            foreach (var interactable in interactablesList)
                interactable.Disable();
    }
    
    protected void FindAllInteractables(Scene scene, LoadSceneMode mode)
    {
        interactablesList = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<InteractableObject>()
            .Except(unaffectedInteractableObjects)
            .ToList();
    }
}
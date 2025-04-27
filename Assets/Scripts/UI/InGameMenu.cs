using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public static Action Clear;
    public static Action Restart;
    public static Action Next;
    public static Action MainMenu;
    public static Action Quit;

    private static InGameMenu _instance;

    public static InGameMenu Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Subscribe(true);
    }

    #region Buttons Behaviour
    public void OnClearClicked()
    {
        Clear?.Invoke();
    }

    public void OnRestartClicked()
    {
        Restart?.Invoke();
    }

    public void OnNextClicked()
    {
        Next?.Invoke();
    }

    public void OnMainMenuClicked()
    {
        MainMenu?.Invoke();
    }

    public void OnExitClicked()
    {
        Quit?.Invoke();
    }
    #endregion

    #region Buttons Logic

    #endregion

    public void PerformActionAfterAnimation(ButtonType button)
    {

    }

    private void Subscribe(bool isListener)
    {
        if (isListener)
        {
            MenuButtonTween.AnimationCompleted += PerformActionAfterAnimation;
            return;
        }
        MenuButtonTween.AnimationCompleted -= PerformActionAfterAnimation;
    }

    #region Enable & Disable
    private void OnEnable()
    {
        Subscribe(true);
    }

    private void OnDisable()
    {
        Subscribe(false);
    }

    private void OnDestroy()
    {
        Subscribe(false);
    }
    #endregion
}

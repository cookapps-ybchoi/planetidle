﻿/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public enum UEReactionType
{
    None = 0,
    Jelly = 1,
    Punch = 11,
    Press = 21,
}

public enum UEButtonSoundType
{
    Default = 0,
    None,
    Custom1,
    Custom2,
    Custom3,
}

public class UEButton : Button
{
    public static Action<UEButton> GlobalClickNotificationEvent;
    
    public void CancelContinue()
    {
        if (this.buttonPressed)
        {
            this.buttonPressed = false;
            this.BackToNormal();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides MonoBehaviour

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this.localScale = this.transform.localScale;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!this.tweenScaleX.Completed)
        {
            float deltaTime = Time.unscaledDeltaTime;
            this.tweenScaleX.Update(deltaTime);
            this.tweenScaleY.Update(deltaTime);

            float scaleX = Mathf.Round(this.tweenScaleXValue.Value / .01f) * .01f;
            float scaleY = Mathf.Round(this.tweenScaleYValue.Value / .01f) * .01f;
            Vector3 scale = new Vector3(scaleX, scaleY, 1f);
            this.transform.localScale = scale;
        }

        if (this.useContinueClick && this.buttonPressed)
        {
            if (Application.isEditor && !Input.GetMouseButton(0))
            {
                this.buttonPressed = false;
                this.BackToNormal();
            }
            else if (!Application.isEditor && Input.touchCount == 0)
            {
                this.buttonPressed = false;
                this.BackToNormal();
            }
            else
            {
                if (this.continueThreshold > 0)
                {
                    this.continueThreshold -= Time.deltaTime;
                }
                else
                {
                    this.ForceClick();
                    this.continueThreshold = Mathf.Max(0.05f, continue_continue_time - (this.continueCount * 0.05f));
                    this.continueCount++;
                }
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!Application.isPlaying) return;

        if (this.localScale == Vector3.zero)
            this.localScale = this.transform.localScale;
        else
            this.transform.localScale = this.localScale;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (!Application.isPlaying) return;

        this.tweenScaleX.Kill();
        this.tweenScaleY.Kill();
        this.transform.localScale = this.localScale;
        this.buttonPressed = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public overrides Button


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.reactionType == UEReactionType.None) return;
        base.OnPointerDown(eventData);
        this.continueCount = 0;
        this.continueThreshold = inital_continue_time;
        this.clickInvoked = false;
        this.cancelClick = false;
        this.buttonPressed = true;

        switch (this.reactionType)
        {
            case UEReactionType.Jelly:
                this.JellyTweenReady();
                break;
            case UEReactionType.Punch:
                this.StartPunchTween();
                break;
            case UEReactionType.Press:
                this.StartPressTween();
                break;
        }

        _onPress.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (this.reactionType == UEReactionType.None) return;
        if (this.cancelClick) return;
        
        if (this.UseContinueClick)
        {
            return;
        }

        if (this.buttonPressed)
        {
            base.OnPointerUp(eventData);
            this.buttonPressed = false;
            switch (this.reactionType)
            {
                case UEReactionType.Jelly:
                    this.StartCoroutine(this.JellyTweenStart());
                    break;
                case UEReactionType.Punch:
                    this.BackToNormal();
                    break;
                case UEReactionType.Press:
                    this.BackToNormal();
                    break;
            }
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this.reactionType == UEReactionType.None) return;
        if (this.UseContinueClick)
        {
            return;
        }

        if (this.buttonPressed)
        {
            base.OnPointerExit(eventData);
            this.cancelClick = true;
            this.buttonPressed = false;
            this.BackToNormal();
        }
    }

    public override void OnMove(AxisEventData eventData)
    {
        if (this.reactionType == UEReactionType.None) return;
        if (this.UseContinueClick)
        {
            return;
        }

        if (this.buttonPressed)
        {
            base.OnMove(eventData);
            this.cancelClick = true;
            this.buttonPressed = false;
            this.BackToNormal();
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.reactionType == UEReactionType.None)
        {
            base.OnPointerClick(eventData);
        }
        else
        {
            this.clickInvoked = false;
            this.ForceClick();
        }
        
        GlobalClickNotificationEvent?.Invoke(this);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    public UEReactionType ReactionType
    {
        get { return this.reactionType; }
        set { this.reactionType = value; }
    }
    
    public UEButtonSoundType SoundType
    {
        get { return this.soundType; }
        set { this.soundType = value; }
    }

    public bool UseContinueClick
    {
        get { return this.useContinueClick; }
        set { this.useContinueClick = value; }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    [SerializeField] private ButtonClickedEvent _onPress = new ButtonClickedEvent();
    
    private const float inital_continue_time = 0.5f;
    private const float continue_continue_time = 0.1f;

    [SerializeField] private bool useContinueClick = false;
    [SerializeField] private UEReactionType reactionType = UEReactionType.None;
    [SerializeField] private UEButtonSoundType soundType = UEButtonSoundType.Default;
    
    private Vector3 localScale;

    private SimpleTweener tweenScaleX = new SimpleTweener();
    private TweenLerp<float> tweenScaleXValue;

    private SimpleTweener tweenScaleY = new SimpleTweener();
    private TweenLerp<float> tweenScaleYValue;

    private bool cancelClick = false;
    private bool clickInvoked = false;
    private bool buttonPressed = false;

    private int continueCount = 0;
    private float continueThreshold = 0f;

    private void ForceClick()
    {
        if (!this.cancelClick && !this.clickInvoked)
        {
            if (!this.useContinueClick)
                this.clickInvoked = true;
            this.onClick.Invoke();
        }
    }

    private void JellyTweenReady()
    {
        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1f, 1.248f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(1f, 0.904f);
    }

    private void StartPunchTween()
    {
        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1f, 1.1f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(1f, 1.1f);
    }    
    
    private void StartPressTween()
    {
        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1f, 0.9f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(1f, 0.9f);
    }

    private void BackToNormal()
    {
        this.tweenScaleX.Reset(0.2f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(this.tweenScaleXValue.Value, 1f);

        this.tweenScaleY.Reset(0.2f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(this.tweenScaleYValue.Value, 1f);
    }

    private IEnumerator JellyTweenStart()
    {
        this.tweenScaleX.Reset(0.1f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1.248f, 0.92f);

        this.tweenScaleY.Reset(0.1f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(0.904f, 1.12f);

        yield return StartCoroutine(this.WaitForRealSeconds(0.15f));

        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(0.92f, 1.112f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(1.12f, 0.888f);

        yield return StartCoroutine(this.WaitForRealSeconds(0.15f));

        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1.112f, 0.944f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(0.904f, 1f);

        yield return StartCoroutine(this.WaitForRealSeconds(0.15f));

        this.tweenScaleX.Reset(0.2f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(0.944f, 1.04f);

        this.tweenScaleY.Reset(0.2f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(1f, 0.976f);

        yield return StartCoroutine(this.WaitForRealSeconds(0.2f));

        this.tweenScaleX.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleXValue = this.tweenScaleX.CreateTween(1.04f, 1f);

        this.tweenScaleY.Reset(0.15f, EasingObject.LinearEasing);
        this.tweenScaleYValue = this.tweenScaleY.CreateTween(0.976f, 1f);

        yield break;
    }

    private IEnumerator WaitForRealSeconds(float seconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            yield return null;
        }
    }
}
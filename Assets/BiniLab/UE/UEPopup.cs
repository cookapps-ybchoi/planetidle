/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UEPopup : UEComponent
{
    private static string UI_POPUP_PATH = "Prefabs/UI/Popups/";
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static

    public static T GetInstantiateComponent<T>() where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(Resources.Load(UI_POPUP_PATH + typeof(T).Name) as GameObject) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }


    public static T GetInstantiateComponent<T>(string path) where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(Resources.Load(path) as GameObject) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }

    public static T GetInstantiateComponent<T>(GameObject prefab) where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }

    public static void HideAll()
    {
        foreach (UEPopup pop in FindObjectsOfType<UEPopup>())
        {
            pop.Hide();
        }
    }

    public static void HideAllForReset()
    {
        foreach (UEPopup pop in FindObjectsOfType<UEPopup>())
        {
            if (pop.isDontDestroy == true)
                continue;

            Destroy(pop.gameObject);
        }
    }

    public static int PopUpCount => popupCount;
    public static bool HasPopup => popupCount > 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected

    protected static UEPopup instance;

    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _showElapsedTime += Time.deltaTime;

        if (!this.bodyScaleTweener.Completed)
        {
            this.bodyScaleTweener.Update(Time.unscaledDeltaTime);
            if (this.popBody) this.popBody.localScale = this.bodyScaleTweenValue.Value;
        }

        if (!this.alphaTweener.Completed)
        {
            this.alphaTweener.Update(Time.unscaledDeltaTime);
            if (this.bodyCanvasGroup) this.bodyCanvasGroup.alpha = this.alphaTweenValue.Value;
        }

        //if (Application.platform == RuntimePlatform.Android)
#if UNITY_ANDROID
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (this.canBackgroundClose && this.countIndex == popupCount && this.completeShow && this.showing)
                    this.Hide();
            }
        }
#endif
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        popupCount++;
        this.countIndex = popupCount;
    }

    protected virtual void OnDisable()
    {
        popupCount--;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    private static int popupCount = 0;
    private int countIndex;

    public delegate void DelOnCompleteShow();

    public delegate void DelOnCompleteHide();

    public virtual void Show(float tweenDuration = DEFAULT_TWEEN_DURATION, bool usingScaleAnim = false, bool usingAlphaAnim = false)
    {
        this.Initialized(usingScaleAnim, usingAlphaAnim);
        if (this.darkBackground != null) this.darkBackground.Show(tweenDuration);

        if (usingScaleAnim)
        {
            this.bodyScaleTweener.Reset(tweenDuration, EasingObject.BackEasingInOut);
            this.bodyScaleTweenValue = this.bodyScaleTweener.CreateTween(this.bodyScaleBeginValue, this.bodyScaleEndValue);
        }

        if (usingAlphaAnim)
        {
            this.alphaTweener.Reset(tweenDuration, EasingObject.LinearEasing, this.OnCompleteShow);
            float startAlpha = this.hasOpenAlpha ? 0.0f : 1.0f;
            this.alphaTweenValue = this.alphaTweener.CreateTween(startAlpha, 1f);
        }

        this.showing = true;
        _showElapsedTime = 0;

        if (!usingScaleAnim && !usingAlphaAnim) OnCompleteShow();
    }

    public virtual void Hide(float tweenDuration = DEFAULT_TWEEN_DURATION, bool usingScaleAnim = false, bool usingAlphaAnim = false)
    {
        if (!this.showing)
            return;

        if (this.darkBackground != null) this.darkBackground.Hide(tweenDuration);

        if (usingScaleAnim)
        {
            this.bodyScaleTweener.Reset(tweenDuration / 2f, EasingObject.BackEasingIn);
            this.bodyScaleTweenValue = this.bodyScaleTweener.CreateTween(this.bodyScaleEndValue, this.bodyScaleBeginValue);
        }

        if (usingAlphaAnim)
        {
            this.alphaTweener.Reset(tweenDuration / 2f, EasingObject.LinearEasing, this.OnCompleteHide);
            this.alphaTweenValue = this.alphaTweener.CreateTween(1f, 0f);
        }
        
        this.showing = false;
        this.initialized = false;
        
        if (!usingScaleAnim && !usingAlphaAnim) OnCompleteHide();
    }

    public void SetOnCompleteShow(DelOnCompleteShow onCompleteShow)
    {
        this.onCompleteShow = onCompleteShow;
    }

    public void SetOnCompleteHide(DelOnCompleteHide onCompleteHide)
    {
        this.onCompleteHide = onCompleteHide;
    }

    public bool Showing
    {
        get { return this.showing; }
    }

    public bool CompleteShow
    {
        get { return this.completeShow; }
    }

    public bool CompleteHide
    {
        get { return this.completeHide; }
    }

    public float GetCameraDepthMax()
    {
        float depth = -100f;
        foreach (var camera in this.cameras)
        {
            depth = Mathf.Max(depth, camera.depth);
        }

        return depth;
    }

    public void SetCameraDepth(float startDepth)
    {
        if (this.cameras == null)
            return;

        for (int i = 0; i < this.cameras.Length; i++)
        {
            this.cameras[i].depth = startDepth + (float)i;
        }
    }

    public virtual bool Closeable
    {
        get { return false; }
    }

    public virtual void OnClickClose()
    {
        SoundManager.Instance.PlayButtonClick();
        this.Hide();
    }

    public virtual void OnClickDarkBackground()
    {
        if (this.canBackgroundClose && this.completeShow && this.showing && _showElapsedTime > DEFAULT_CAN_CLOSE_DURATION)
        {
            SoundManager.Instance.PlayCancel();
            this.Hide();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected
    protected const float DEFAULT_TWEEN_DURATION = 0.2f;

    protected const float DEFAULT_CAN_CLOSE_DURATION = 1.0f;

    protected float _showElapsedTime = 0;

    //UI Components
    [SerializeField] protected UEPopupBackground darkBackground;
    [SerializeField] protected RectTransform popBody;
    [SerializeField] protected Camera[] cameras;
    [SerializeField] protected bool canBackgroundClose = true;
    [SerializeField] protected bool hasOpenAlpha = true;
    [SerializeField] protected bool isDontDestroy = false;

    protected SimpleTweenerEx bodyScaleTweener = new SimpleTweenerEx();
    protected TweenLerp<Vector3> bodyScaleTweenValue;
    protected Vector3 bodyScaleBeginValue = Vector3.zero;
    protected Vector3 bodyScaleEndValue = Vector3.one;

    protected SimpleTweenerEx alphaTweener = new SimpleTweenerEx();
    protected TweenLerp<float> alphaTweenValue;

    protected DelOnCompleteShow onCompleteShow;
    protected DelOnCompleteHide onCompleteHide;

    protected bool showing = false;
    protected bool completeShow = false;
    protected bool completeHide = false;
    protected bool initialized = false;

    protected CanvasGroup bodyCanvasGroup;

    //out of spac
    //protected bool closeable = false;

    protected virtual void OnCompleteShow(object[] onCompleteParms = null)
    {
        this.completeShow = true;
        onCompleteShow?.Invoke();
    }

    protected virtual void OnCompleteHide(object[] onCompleteParms = null)
    {
        this.completeHide = true;
        this.gameObject.SetActive(false);
        this.onCompleteHide?.Invoke();

        if (this.isDontDestroy == false)
            Destroy(this.gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    private void Initialized(bool usingScaleAnim, bool usingAlphaAnim)
    {
        if (this.initialized)
            return;

        this.gameObject.SetActive(true);
        if (this.popBody)
        {
            this.bodyCanvasGroup = this.popBody.GetComponent<CanvasGroup>();
            
            if (usingAlphaAnim)
                this.bodyCanvasGroup.alpha = 0f;
            else
                this.bodyCanvasGroup.alpha = 1f;

            if (usingScaleAnim)
                this.popBody.transform.localScale = Vector3.zero;
            else
                this.popBody.transform.localScale = Vector3.one;
        }

        this.initialized = true;
    }
}
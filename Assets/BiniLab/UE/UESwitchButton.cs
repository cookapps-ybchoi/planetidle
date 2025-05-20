/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class UESwitchEvent : UnityEvent<bool> { }

public class UESwitchButton : MonoBehaviour, IPointerClickHandler
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    //IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        this.isOn = !this.isOn;
        onSwitch.Invoke(this.isOn);
        this.SetUI();
    }

    public void SetOnOff(bool onOff)
    {
        if (this.isOn != onOff)
        {
            this.isOn = onOff;
            this.SetUI();
        }
    }

    public void ChangeOnOff(bool onOff)
    {
        if (this.isOn != onOff)
        {
            this.isOn = onOff;
            this.SetUI();
        }
    }

    public bool IsOn { get { return this.isOn; } }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Life Cycle

    void Start()
    {
        this.SetUI();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    [SerializeField] private GameObject enabledObj;
    [SerializeField] private GameObject disabledObj;
    [SerializeField] private Slider slider;

    [SerializeField] private UESwitchEvent onSwitch;

    [SerializeField] private bool isOn = false;

    private void SetUI()
    {
        this.enabledObj.SetActive(this.isOn);
        this.disabledObj.SetActive(!this.isOn);

        if (this.slider)
        {
            if (this.gameObject.activeInHierarchy)
            {
                if (this.isOn)
                {
                    if (this.slider.value != 1f) this.StartCoroutine(this.MoveSlider(0f, 1f));
                }
                else
                {
                    if (this.slider.value != 0f) this.StartCoroutine(this.MoveSlider(1f, 0f));
                }
            }
            else
            {
                this.slider.value = this.isOn ? 1f : 0f;
            }
        }
    }

    private IEnumerator MoveSlider(float start, float end)
    {
        float accTime = 0f;
        float duration = 0.2f;

        while (accTime < duration)
        {
            this.slider.value = Mathf.LerpUnclamped(start, end, accTime / duration);
            accTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.slider.value = end;
    }

}

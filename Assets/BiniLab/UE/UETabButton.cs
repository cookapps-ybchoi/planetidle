/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class UETabEvent : UnityEvent<int> {}

public enum UETabSoundType
{
	Default = 0,
	None,
	Custom1,
	Custom2,
	Custom3,
}

public class UETabButton : MonoBehaviour, IPointerClickHandler
{
	public static Action<UETabButton> GlobalTabNotificationEvent;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public
	public UETabSoundType SoundType
	{
		get { return this.soundType; }
		set { this.soundType = value; }
	}
	
	//IPointerClickHandler
	public void OnPointerClick( PointerEventData eventData )
	{
		onTab.Invoke(this.index);
		GlobalTabNotificationEvent?.Invoke(this);
	}

	public void SetSelected(bool isSelected)
	{
		this.isSelected = isSelected;
		this.UpdateUI ();
	}

	public void SetIndex(int index)
	{
		this.index = index;
	}

	public void AddTapClickEvent(UnityAction<int> evt)
	{
		this.onTab.AddListener(evt);
	}

	public int Index { get { return this.index; } }
	public bool IsSelected { get { return this.isSelected; } }

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// Life Cycle

	void Start()
	{
		this.UpdateUI ();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private

	[SerializeField] private GameObject enabledObj;
	[SerializeField] private GameObject disabledObj;

	[SerializeField] private UETabEvent onTab;

	[SerializeField] private int index;

	[SerializeField] private UETabSoundType soundType;

	[SerializeField] private bool isSelected = false;

	private void UpdateUI()
	{
		if(this.enabledObj) this.enabledObj.SetActive (this.isSelected);
		if(this.disabledObj) this.disabledObj.SetActive (!this.isSelected);
	}

}

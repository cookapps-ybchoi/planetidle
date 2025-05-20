using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]
public class FlowText : MonoBehaviour
{
    [Range(1f, 100f)] [SerializeField] private float _speed = 50f;
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _scrollDelay = 1f;
    [SerializeField] private RectTransform _textTransform;

    private RectTransform _rectTransform;

    protected void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();

        if (_rectTransform.sizeDelta.x < _textTransform.sizeDelta.x)
        {
            StartCoroutine(RollText());
        }
    }

    private IEnumerator RollText()
    {
        _textTransform.anchoredPosition = Vector2.zero;

        float minPosX = -_textTransform.sizeDelta.x;
        float maxPosX = _rectTransform.sizeDelta.x;

        yield return new WaitForSecondsRealtime(_startDelay);

        float posX = 0;
        bool startOver = false;
        while (true)
        {
            posX -= _speed * Time.unscaledDeltaTime;
            if (posX < minPosX)
            {
                posX = maxPosX;
                startOver = true;
            }

            if (startOver && posX < 0f)
            {
                posX = 0f;
                startOver = false;
                _textTransform.anchoredPosition = new Vector2(posX, 0f);
                yield return new WaitForSecondsRealtime(_scrollDelay);
            }
            else
            {
                _textTransform.anchoredPosition = new Vector2(posX, 0f);
            }


            yield return null;
        }
    }
}
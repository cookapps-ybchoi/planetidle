using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class RandomColor : MonoBehaviour
{
    [SerializeField] private float _timeTochange = 0.1f;
    [SerializeField] private bool _usePivotColor = false;
    [SerializeField] private Color _pivotColor = Color.white;
    private Image _image;
    private Text _text;

    private float _timeSinceChange = 0f;
    private bool _isPivot = false;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _timeSinceChange += Time.unscaledDeltaTime;

        if (_timeSinceChange >= _timeTochange)
        {
            if (_usePivotColor && _isPivot)
            {
                UpdateColor(_pivotColor);
            }
            else
            {
                UpdateColor(new Color(Random.value, Random.value, Random.value));
            }

            _timeSinceChange -= _timeTochange;
        }
    }

    private void UpdateColor(Color color)
    {
        if (_text) _text.color = color;
        if (_image) _image.color = color;
    }
}
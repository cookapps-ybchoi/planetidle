using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class BaseUI : MonoBehaviour
{
    private CanvasGroup _body;
    [SerializeField] protected CanvasGroup[] _components;

    protected virtual void Awake()
    {
        _body = GetComponent<CanvasGroup>();
    }

    public virtual void Show(bool usingScale = true)
    {
        gameObject.SetActive(true);

        _body.alpha = 0;
        foreach (var component in _components)
        {
            component.alpha = 0;
            if (usingScale)
            {
                component.transform.localScale = Vector3.zero;
            }
        }

        _body.DOFade(1f, Constants.UI_FADE_TIME).SetEase(Constants.UI_FADE_EASE_DEFAULT).SetUpdate(true).onComplete = () =>
        {
            for (int i = 0; i < _components.Length; i++)
            {
                _components[i].DOFade(1f, Constants.UI_FADE_TIME).SetEase(Constants.UI_FADE_EASE_DEFAULT).SetDelay(i * Constants.UI_FADE_DELAY).SetUpdate(true);
                if (usingScale)
                {
                    _components[i].transform.DOScale(Vector3.one, Constants.UI_FADE_TIME).SetEase(Ease.OutBack).SetDelay(i * Constants.UI_FADE_DELAY).SetUpdate(true);
                }
            }
        };
    }

    public virtual void Hide()
    {
        if (!gameObject.activeSelf) return;

        _body.DOFade(0, Constants.UI_FADE_TIME).SetEase(Constants.UI_FADE_EASE_DEFAULT).SetUpdate(true).onComplete = () =>
        {
            gameObject.SetActive(false);
        };
    }
}
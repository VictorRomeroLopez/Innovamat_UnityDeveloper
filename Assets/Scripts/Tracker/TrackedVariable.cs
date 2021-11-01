using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrackedVariable : MonoBehaviour
{
    #region Events

    public UnityAction OnTextUpdated = delegate { };
    
    #endregion
    
    #region Serialized Fields

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _rectTransform;

    #endregion

    #region Standard Attributes

    private int _value;
    private string _name;

    #endregion

    #region API Methods

    public void Configure(string variableName)
    {
        _name = variableName;
        name = $"{variableName}_{name}";
        UpdateText();
    }
    
    public void Increase(int value = 1)
    {
        _value += value;
        UpdateText();
    }

    public void Decrease(int value = 1)
    {
        _value -= value;
        UpdateText();
    }

    #endregion

    #region Other Methods

    private void UpdateText()
    {
        _text.text = $"{_name}: {_value}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        OnTextUpdated.Invoke();
    }

    #endregion
  
}
using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Activity
{
    public class OptionBox : MonoBehaviour
    {
        #region Events

        public delegate void CustomActionInt(int value);
        
        public CustomActionInt OnCorrectAnswer = delegate { };
        public CustomActionInt OnWrongAnswer = delegate { };

        #endregion

        #region Constants

        private const float FADE_DURATION = .5f;
        
        #endregion

        #region Serialized Fields

        [SerializeField] private FadeAnimator _fadeAnimator;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        #endregion

        #region Standard Attributes
        
        private bool _isCorrect = true;
        private int _id;
        
        #endregion

        #region Properties

        public FadeAnimator FadeAnimator => _fadeAnimator;
        public bool IsShowing => Math.Abs(_canvasGroup.alpha - 1) < Mathf.Epsilon;
        
        #endregion

        #region API Methods

        public void Configure(int id, int number, bool isCorrect = false)
        {
            _id = id;
            _textMesh.text = number.ToString();
            _isCorrect = isCorrect;
            _background.color = Color.white;
        }
        
        public void Show()
        {
            _fadeAnimator.FadeIn();
        }

        public void Hide()
        {
            _fadeAnimator.FadeOut();
        }

        public void ColorBox()
        {
            if (_isCorrect)
            {
                _background.color = Color.green;
                return;
            }
            
            _background.color = Color.red;
        }
        
        public void InvokeResult()
        {
            if (_isCorrect)
            {
                OnCorrectAnswer.Invoke(_id);
                return;
            }
            
            OnWrongAnswer.Invoke(_id);
        }

        public void Lock()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Unlock()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _fadeAnimator.Configure(FADE_DURATION);
        }

        private void OnEnable()
        {
            _fadeAnimator.OnFadeInCompleted += Unlock;
            _button.onClick.AddListener(ColorBox);
            _button.onClick.AddListener(InvokeResult);
        }

        private void OnDisable()
        {
            _fadeAnimator.OnFadeInCompleted -= Unlock;
            _button.onClick.RemoveListener(ColorBox);
            _button.onClick.RemoveListener(InvokeResult);
        }

        #endregion
    }
}

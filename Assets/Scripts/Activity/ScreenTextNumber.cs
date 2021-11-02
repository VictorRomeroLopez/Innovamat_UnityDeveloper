using System.Collections;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Activity
{
    public class ScreenTextNumber : MonoBehaviour
    {
        #region Events

        public UnityAction OnNumberFinishedAnimation = delegate {  };
        
        #endregion

        #region Constants

        private const float WAIT_DURATION = 2;
        private const float FADE_DURATION = 2;
        
        #endregion

        #region Serialized Fields

        [SerializeField] private FadeAnimator _fadeAnimator;
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        #endregion

        #region Standard Attributes

        private WaitForSeconds _waitBeforeFadeOut;

        #endregion

        #region API Methods

        public void Configure(string stringedNumber)
        {
            _textMesh.text = stringedNumber;
        }

        public void Show()
        {
            _fadeAnimator.FadeIn();
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _waitBeforeFadeOut = new WaitForSeconds(WAIT_DURATION);
            _fadeAnimator.Configure(FADE_DURATION);
        }

        private void OnEnable()
        {
            _fadeAnimator.OnFadeInCompleted += FadeOut;
            _fadeAnimator.OnFadeOutCompleted += FadeOutCompleted;
        }

        private void OnDisable()
        {
            _fadeAnimator.OnFadeInCompleted -= FadeOut;
            _fadeAnimator.OnFadeOutCompleted -= FadeOutCompleted;
        }

        #endregion

        #region Other Methods

        private void FadeOut()
        {
            StartCoroutine(WaitAndFadeOut());
        }

        private IEnumerator WaitAndFadeOut()
        {
            yield return _waitBeforeFadeOut;
            _fadeAnimator.FadeOut();
        }

        private void FadeOutCompleted()
        {
            OnNumberFinishedAnimation.Invoke();
        }

        #endregion
    }
}
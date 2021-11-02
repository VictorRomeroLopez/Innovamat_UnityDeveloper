using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeAnimator : MonoBehaviour
    {
        #region Events

        public UnityAction OnFadeInCompleted = delegate {};
        public UnityAction OnFadeOutCompleted = delegate {};
        
        #endregion

        #region Serialized Fields

        [SerializeField] private CanvasGroup _canvasGroup;
        
        #endregion

        #region Standard Attributes

        private float _fadeInDuration = 2; 
        private float _fadeOutDuration = 2;
        
        #endregion

        #region Properties

        public bool IsFading { get; private set; }

        #endregion

        #region API Methods

        public void Configure(float fadeDuration)
        {
            _fadeInDuration = fadeDuration;
            _fadeOutDuration = fadeDuration;
        }
        
        public void FadeIn()
        {
            IsFading = true;
            
            StartCoroutine(Tween(_fadeInDuration, 0, 1, () =>
            {
                OnFadeInCompleted.Invoke();
                IsFading = false;
            }));
        }

        public void FadeOut()
        {
            IsFading = true;
            StartCoroutine(Tween(_fadeOutDuration, 1, 0, () =>
            {
                OnFadeOutCompleted.Invoke();
                IsFading = false;
            }));
        }
        
        #endregion

        #region Other Methods
        
        private IEnumerator Tween(float time, int from, int to, Action onCompleteAction)
        {
            float timeCollected = 0;
            
            do
            {
                timeCollected += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(from, to, timeCollected/time);
                yield return new WaitForEndOfFrame();
            } while (timeCollected/time < 1);
            _canvasGroup.alpha = to;
            
            onCompleteAction.Invoke();
        }

        #endregion
        
    }
}
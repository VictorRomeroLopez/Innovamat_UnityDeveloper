using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Activity
{
    public class OptionBoxesHandler : MonoBehaviour
    {
        #region Events

        public UnityAction OnAllButtonsHidden = delegate {};
        
        #endregion

        #region Serialized Fields

        [SerializeField] private List<OptionBox> _optionBoxes;

        #endregion

        #region Standard Attributes

        private List<OptionBox> _remainingOptions;

        #endregion

        #region Properties

        public List<OptionBox> OptionBoxes => _optionBoxes;

        #endregion

        #region API Methods

        public void Configure()
        {
            _remainingOptions = new List<OptionBox>(_optionBoxes);
            
            foreach (OptionBox optionBox in _optionBoxes)
            {
                optionBox.FadeAnimator.OnFadeOutCompleted -= UnlockRemainingBoxes;
            }
        }

        public void LockRemainingBoxes()
        {
            foreach (OptionBox optionBox in _remainingOptions)
            {
                optionBox.Lock();
            }
        }

        public void RemoveOption(int idBox)
        {
            _remainingOptions.Remove(_optionBoxes[idBox]);
        }

        public void Solve()
        {
            foreach (OptionBox remainingOption in _remainingOptions)
            {
                remainingOption.ColorBox();
            }
        }

        public void HideBox(int idBox)
        {
            _optionBoxes[idBox].Hide();
        }

        public void UnlockOnFinishFadeOut(int idBox)
        {
            _optionBoxes[idBox].FadeAnimator.OnFadeOutCompleted += UnlockRemainingBoxes;
        }
        
        public void UnlockRemainingBoxes()
        {
            foreach (OptionBox remainingOption in _remainingOptions)
            {
                remainingOption.Unlock();
            }
        }

        public void ShowAllBoxes()
        {
            foreach (OptionBox optionBox in _optionBoxes)
            {
                optionBox.Show();
            }
        }

        public void HideRemainingBoxes()
        {
            foreach (OptionBox optionBox in _remainingOptions)
            {
                optionBox.Hide();
            }
        }

        public void CheckAllBoxesHidden()
        {
            foreach (OptionBox remainingOption in _remainingOptions)
            {
                if (remainingOption.IsShowing)
                {
                    return;
                }
            }
            
            OnAllButtonsHidden.Invoke();
        }
        
        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            foreach (OptionBox optionBox in _optionBoxes)
            {
                optionBox.FadeAnimator.OnFadeOutCompleted += CheckAllBoxesHidden;
            }
        }

        private void OnDisable()
        {
            foreach (OptionBox optionBox in _optionBoxes)
            {
                optionBox.FadeAnimator.OnFadeOutCompleted -= CheckAllBoxesHidden;
            }
        }

        #endregion
    }
}
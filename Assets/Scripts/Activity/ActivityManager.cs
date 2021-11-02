using System.Collections;
using System.Collections.Generic;
using Tracker;
using UnityEngine;

namespace Activity
{
    public class ActivityManager : MonoBehaviour
    {
        #region Constants

        private const float ANSWER_DELAY_DURATION = 2;
        
        #endregion

        #region Serialized Fields

        [SerializeField] private TrackerBoard _trackerBoard;
        [SerializeField] private int _activityNumber;
        [SerializeField] private List<ActivitySettings> _settings;
        [SerializeField] private ScreenTextNumber _screenTextNumber;
        [SerializeField] private OptionBoxesHandler _boxesCollection;

        #endregion

        #region Standard Attributes
        
        private ActivityConfigurator _activityConfigurator;
        private bool _hasHint = true;
        private WaitForSeconds _waitAfterAnswer;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _waitAfterAnswer = new WaitForSeconds(ANSWER_DELAY_DURATION);
            _activityConfigurator = new ActivityConfigurator();
            StartActivity();
        }

        private void OnEnable()
        {
            _screenTextNumber.OnNumberFinishedAnimation += _boxesCollection.ShowAllBoxes;
            _boxesCollection.OnAllButtonsHidden += StartActivity;
            
            foreach (OptionBox optionBox in _boxesCollection.OptionBoxes)
            {
                optionBox.OnCorrectAnswer += OnCorrectAnswerSelected;
                optionBox.OnWrongAnswer += OnWrongAnswerSelected;
            }
        }

        private void OnDisable()
        {
            _screenTextNumber.OnNumberFinishedAnimation -= _boxesCollection.ShowAllBoxes;
            _boxesCollection.OnAllButtonsHidden -= StartActivity;
            
            foreach (OptionBox optionBox in _boxesCollection.OptionBoxes)
            {
                optionBox.OnCorrectAnswer -= OnCorrectAnswerSelected;
                optionBox.OnWrongAnswer -= OnWrongAnswerSelected;
            }
        }

        #endregion

        #region Other Methods

        private void StartActivity()
        {
            _hasHint = true;
            _boxesCollection.Configure();
            _activityConfigurator.Configure(_settings[_activityNumber], _screenTextNumber, _boxesCollection.OptionBoxes);
            _screenTextNumber.Show();
        }

        private void OnCorrectAnswerSelected(int idBox)
        {
            _boxesCollection.LockRemainingBoxes();
            _trackerBoard.VariableTypeToTrackedVariable[TrackerVariableType.Succes].Increase();
            StartCoroutine(WaitAndHideSuccess());
        }

        private IEnumerator WaitAndHideSuccess()
        {
            yield return _waitAfterAnswer;
            _boxesCollection.HideRemainingBoxes();
        }

        private void OnWrongAnswerSelected(int idBox)
        {
            _boxesCollection.LockRemainingBoxes();
            _boxesCollection.RemoveOption(idBox);
            
            if (!_hasHint)
            {
                _trackerBoard.VariableTypeToTrackedVariable[TrackerVariableType.Failure].Increase();
                _boxesCollection.Solve();
            }
            
            StartCoroutine(WaitAndHideWrong(idBox));
        }

        private IEnumerator WaitAndHideWrong(int idBox)
        {
            yield return _waitAfterAnswer;

            _boxesCollection.HideBox(idBox);
            
            if (_hasHint)
            {
                _boxesCollection.UnlockOnFinishFadeOut(idBox);
                _hasHint = false;
            }
            else
            {
                _boxesCollection.HideRemainingBoxes();
            }
        }

        #endregion
    }
}

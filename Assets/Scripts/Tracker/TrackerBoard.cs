using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tracker
{
    [RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(RectTransform))]
    public class TrackerBoard : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private List<TrackerVariableType> _variablesToTrack;
        [SerializeField] private TrackedVariable _trackedVariablePrefab;

        #endregion

        #region Standard Attributes

        private VerticalLayoutGroup _verticalLayoutGroup;
        private ContentSizeFitter _contentSizeFitter;
        private RectTransform _rectTransform;

        #endregion

        #region Properties

        public Dictionary<TrackerVariableType, TrackedVariable> VariableTypeToTrackedVariable { get; private set; }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
            _rectTransform = GetComponent<RectTransform>();

            VariableTypeToTrackedVariable = new Dictionary<TrackerVariableType, TrackedVariable>();
            
            foreach (TrackerVariableType trackerVariableType in _variablesToTrack)
            {
                TrackedVariable instance = Instantiate(_trackedVariablePrefab, _rectTransform);
                instance.Configure(trackerVariableType.ToString());
                VariableTypeToTrackedVariable.Add(trackerVariableType, instance);
            }

            RebuildLayout();
        }

        private void OnEnable()
        {
            foreach (KeyValuePair<TrackerVariableType,TrackedVariable> trackedVariable in VariableTypeToTrackedVariable)
            {
                trackedVariable.Value.OnTextUpdated += RebuildLayout;
            }
        }

        private void OnDisable()
        {
            foreach (KeyValuePair<TrackerVariableType,TrackedVariable> trackedVariable in VariableTypeToTrackedVariable)
            {
                trackedVariable.Value.OnTextUpdated -= RebuildLayout;
            }
        }

        #endregion

        #region Other Methods

        private void RebuildLayout()
        {
            EnableContentFitterElements();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            DisableContentFitterElements();
        }
        
        private void EnableContentFitterElements()
        {
            _verticalLayoutGroup.enabled = true;
            _contentSizeFitter.enabled = true;
        }

        private void DisableContentFitterElements()
        {
            _verticalLayoutGroup.enabled = false;
            _contentSizeFitter.enabled = false;
        }

        #endregion
    }
}
using UnityEngine;

namespace Activity
{   
    [CreateAssetMenu(fileName = "ActivitySettings", menuName = "Bmath/ActivitySettings", order = 1)]
    public class ActivitySettings : ScriptableObject
    {
        [Header("Range statements")]
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public int MinRangeStatement => _min;
        public int MaxRangeStatement => _max;

        private void OnValidate()
        {
            if (_min <= 0)
            {
                Debug.LogWarning("Min value has to be greater than 0");
                _min = 1;
            }

            if (_max <= 0)
            {
                Debug.LogWarning("Max value has to be greater than 0");
                _max = 4;
            }
            
            if (_max - _min < 3)
            {
                Debug.LogWarning("Range between min and max has to be greater than 3");
                _max = _min + 3;
            }
        }
    }
}
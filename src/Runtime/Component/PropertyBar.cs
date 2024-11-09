using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore.Component
{
    enum BarType
    {
        LeftToRight,
        RightToLeft,
    }
    [AddComponentMenu("PFC/PropertyBar")]
    public class PropertyBar : MonoBehaviour
    {
        // [SerializeField] private Transform leftPoint;
        // [SerializeField] private Transform rightPoint;
        [SerializeField] private BarType barType;
        [SerializeField] private Mask barMask;
        [SerializeField] private Image barImage;
        private Vector3 _edgePosition;
        public Vector3 EdgePosition => _edgePosition;
        private Vector3 _initImagePosition;
        public Vector3 InitImagePosition => _initImagePosition;
        private float _value;

        public float Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        private void Awake()
        {
            _initImagePosition = barImage.transform.position;
        }
        
        public void SetValue(float value)
        {
            _value = Mathf.Clamp01(value);
            var leftPosition = barImage.rectTransform.position - barImage.rectTransform.rect.width / 2 * barImage.rectTransform.right;
            var rightPosition = barImage.rectTransform.position + barImage.rectTransform.rect.width / 2 * barImage.rectTransform.right;
            var center = (leftPosition + rightPosition) / 2;
            if (barType == BarType.RightToLeft)
            {
                barMask.transform.position = Vector3.Lerp(center, leftPosition + (leftPosition - rightPosition) / 2, _value);
                barImage.transform.position = _initImagePosition;
                _edgePosition = Vector3.Lerp(rightPosition, leftPosition, _value);
            }
            else
            {
                barMask.transform.position = Vector3.Lerp(center, rightPosition + (rightPosition - leftPosition) / 2, _value);
                barImage.transform.position = _initImagePosition;
                _edgePosition = Vector3.Lerp(leftPosition, rightPosition, _value);
            }
        }

        public float GetValue()
        {
            return _value;
        }

    }
}
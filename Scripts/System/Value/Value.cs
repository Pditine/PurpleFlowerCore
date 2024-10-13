using System;

namespace PurpleFlowerCore.Utility
{
    public class Value<T>
    {
        /// <summary>
        /// 标识, 目前仅用于调试, 未来可能会用于其他用途
        /// </summary>
        private string _name;
        /// <summary>
        /// 具体值
        /// </summary>
        private T _value;
        /// <summary>
        /// 是否可读, 默认为true
        /// </summary>
        private bool _canRead;
        /// <summary>
        /// 是否可写, 默认为true
        /// </summary>
        private bool _canWrite;
        /// <summary>
        /// 是否可传入更改,如+= *=等, 默认为true
        /// </summary>
        private bool _canChange;
        private T _min;
        private T _max;
        public event Action<T,T> OnValueChange;
        public event Action<T,T> OnValueChanged;
        public event Action<T> OnValueWrite;
        public event Action<T,T> OnValueAdd;
        public event Action<T,T> OnValueSub;
        public event Action<T,T> OnValueMul;
        public event Action<T,T> OnValueDiv;
        public event Action<T> OnValueRead;
        public event Action<T> OnValueReachMin;
        public event Action<T> OnValueReachMax;

        public T Min
        {
            get => _min;
            set
            {
                if (CompareValues(value, _max) > 0) throw new Exception("MinValue must be less than MaxValue");
                _min = value;
            }
        }
        
        public Value(string name = default, T value = default, bool canRead = true, bool canWrite = true, bool canChange = true, T min = default, T max = default)
        {
            _name = name;
            _value = value;
            _canRead = canRead;
            _canWrite = canWrite;
            _canChange = canChange;
            if (IsIComparable<T>())
            {
                _min = min;
                _max = max;
                if (CompareValues(value, min) < 0)
                {
                    // todo: 初始化时事件为空
                    OnValueReachMin?.Invoke(value);
                    _value = min;
                }
                if (CompareValues(value, max) > 0)
                {
                    OnValueReachMax?.Invoke(value);
                    _value = max;
                }
            }
            else
            {
                _min = default;
                _max = default;
                throw new Exception("Value must be IComparable");
            }

        }
        
        // todo: 该属性存在的必要性,是否可以被运算符重载替代
        public T TheValue
        {
            get
            {
                if(!_canRead) throw new Exception($"Can't read this value: {_name}");
                return _value;
            }
            set
            {
                if(!_canWrite) throw new Exception($"Can't write this value: {_name}");
                if(IsIComparable<T>()) throw new Exception($"Value must be greater than {_min}");
                OnValueChange?.Invoke(_value, value);
                _value = value;
                OnValueChanged?.Invoke(_value, value);
            }
        }
        
        private static bool IsIComparable<T>()
        {
            return typeof(IComparable).IsAssignableFrom(typeof(T)) ||
                   typeof(IComparable<T>).IsAssignableFrom(typeof(T));
        }
        
        private static int CompareValues<T>(T x, T y)
        {
            if (x is IComparable comparableX)
            {
                return comparableX.CompareTo(y);
            }

            throw new ArgumentException($"{typeof(T).FullName} must implement IComparable.");
        }
    }
}
namespace PurpleFlowerCore
{
    public class WaitForSecond: IProcessNode
    {
        private readonly float _waitTime;
        private float _currentTime;
        public WaitForSecond(float time)
        {
            _waitTime = time;
        }
        public bool Update(float deltaTime)
        {
            _currentTime += deltaTime;
            return _currentTime > _waitTime;
        }

        public void ReSet()
        {
            _currentTime = 0;
        }

        public static implicit operator WaitForSecond(float time)
        {
            return new WaitForSecond(time);
        }
    }
}
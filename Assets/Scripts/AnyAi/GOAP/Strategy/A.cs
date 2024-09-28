using System.Threading;

namespace GOAP
{
    public class A : IActionStrategy
    {
        public bool CanPerform { get; }
        public bool Complete { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections;

namespace States
{
    public interface IState
    {
        public IEnumerator Start();
    }
}
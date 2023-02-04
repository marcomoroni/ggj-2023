using UnityEngine;

namespace GGJ2023
{
    public class GameplayManager : MonoBehaviour
    {
        private abstract class State
        {
            public virtual void OnEnter() { }
            public virtual void OnExit() { }
        }

        private class MenuState
        {

        }
    }
}

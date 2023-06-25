using System;

namespace Tonvo.Core
{
    internal class EventManager
    {
        public static event Action Validated;

        public static void OnValidated()
        {
            Validated?.Invoke();
        }
    }
}

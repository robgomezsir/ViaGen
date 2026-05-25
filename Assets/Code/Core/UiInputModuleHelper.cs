using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace ViaGen.Core
{
    public static class UiInputModuleHelper
    {
        public static void EnsureCorrectModule()
        {
            if (EventSystem.current == null)
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
                es.AddComponent<InputSystemUIInputModule>();
#else
                es.AddComponent<StandaloneInputModule>();
#endif
                return;
            }

#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.GetComponent<InputSystemUIInputModule>() == null &&
                EventSystem.current.GetComponent<StandaloneInputModule>() != null)
            {
                Object.Destroy(EventSystem.current.GetComponent<StandaloneInputModule>());
                EventSystem.current.gameObject.AddComponent<InputSystemUIInputModule>();
            }
#endif
        }
    }
}

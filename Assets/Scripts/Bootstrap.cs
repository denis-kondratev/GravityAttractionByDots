using UnityEngine;

namespace GravityAttraction
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeOnStart()
        {
            Application.targetFrameRate = 60;
        }
    }
}

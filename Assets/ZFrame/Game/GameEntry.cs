using UnityEngine;

namespace ZFrame
{
    /// <summary>
    /// This is the game entry class
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Start()
        {
        }

        private void OnApplicationPause()
        {
        }

        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF3DRacing
{
    public class SceneLoader : MonoBehaviour
    {
        private const string MainMenuScene = "MenuScene";

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

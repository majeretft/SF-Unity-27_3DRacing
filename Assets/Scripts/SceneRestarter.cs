using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF3DRacing
{
    public class SceneRestarter : MonoBehaviour
    {
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

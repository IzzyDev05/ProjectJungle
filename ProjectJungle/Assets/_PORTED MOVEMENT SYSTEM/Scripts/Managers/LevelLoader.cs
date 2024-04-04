using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private GameObject winCam;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        winCam.SetActive(false);
    }

    public void LoadLevel(int buildIndex)
    {
        StartCoroutine(LoadLevelRoutine(buildIndex));
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelRoutine(currentSceneIndex + 1));
    }

    private IEnumerator LoadLevelRoutine(int buildIndex)
    {
        // Start animation
        animator.SetTrigger("FadeOut");
        
        // Wait
        yield return new WaitForSeconds(fadeOutTime);

        // Load scene
        SceneManager.LoadScene(buildIndex);
    }

    public void WinGame()
    {
        winCam.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
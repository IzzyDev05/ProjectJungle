using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 1f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

        // Play level load sound
        AudioManager.Instance.PlayLevelChangeSound();

        // Wait
        yield return new WaitForSeconds(fadeOutTime);

        // Load scene
        SceneManager.LoadScene(buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
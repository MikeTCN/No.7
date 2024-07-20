using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FixedTimedSceneChanger : MonoBehaviour
{
    public float delayBeforeTransition = 5f; // 場景切換前的等待時間（秒）
    public int sceneToLoad; // 要加載的場景索引
    public SceneTransitionManager sceneTransitionManager; // 場景轉換管理器的引用

    private void Start()
    {
        StartCoroutine(DelayedSceneChange());
    }

    private IEnumerator DelayedSceneChange()
    {
        // 等待指定的時間
        yield return new WaitForSeconds(delayBeforeTransition);

        // 使用 SceneTransitionManager 進行場景切換
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.GoToSceneAsync(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager is not assigned. Loading scene directly.");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
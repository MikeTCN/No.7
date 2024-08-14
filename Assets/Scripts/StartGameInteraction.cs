using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StartGameInteraction : MonoBehaviour
{
    public GameObject startScreen; // 开始画面
    public int sceneToLoad;
    public AudioSource audioSource; // AudioSource 组件引用
    public SceneTransitionManager sceneTransitionManager; // 场景转换管理器

    private XRGrabInteractable grabInteractable;
    private bool isGameStarted = false; // 标记游戏是否已开始

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(StartGame);

        // 检查是否已设置 AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("未設置 AudioSource！請在 Inspector 中指定一個 AudioSource 組件。 ⚠️");
            }
        }

        // 确保音频源在开始时是停止的
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.playOnAwake = false;
        }

        // 检查是否已设置 SceneTransitionManager
        if (sceneTransitionManager == null)
        {
            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            if (sceneTransitionManager == null)
            {
                Debug.LogError("場景中沒有 SceneTransitionManager！請確保場景中有此組件。 ❌");
            }
        }
    }

    private void StartGame(SelectEnterEventArgs args)
    {
        if (!isGameStarted)
        {
            isGameStarted = true;

            // 播放空间音频
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("物體被抓起，開始播放空間音頻 🎵");
            }
            else if (audioSource == null)
            {
                Debug.LogError("AudioSource 未設置，無法播放音頻！ ❌");
            }

            startScreen.SetActive(false);

            // 使用 SceneTransitionManager 进行场景转换
            if (sceneTransitionManager != null)
            {
                sceneTransitionManager.GoToSceneAsync(sceneToLoad);
            }
            else
            {
                Debug.LogError("SceneTransitionManager 未設置，無法進行場景轉換！ ❌");
            }
        }
    }
}
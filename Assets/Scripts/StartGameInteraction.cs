using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Threading.Tasks;

public class StartGameInteraction : MonoBehaviour
{
    public GameObject startScreen; // 开始画面
    public int SceneToLoad;

    private XRGrabInteractable grabInteractable;

    private bool isGameStarted = false; // 标记游戏是否已开始

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(StartGame);
    }

    private async void StartGame(SelectEnterEventArgs args)
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            startScreen.SetActive(false);
            // 使用异步加载场景的方法
            await LoadNewSceneAsync();
        }
    }

    private async Task LoadNewSceneAsync()
    {
        // 可以在这里添加过渡效果，比如淡出效果
        await Task.Delay(1000); // 等待1秒，可以根据需要调整

        // 异步加载新场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        asyncOperation.allowSceneActivation = false; // 禁止自动激活新场景

        while (!asyncOperation.isDone)
        {
            // 在加载完成前，可以添加过渡效果或其他操作

            // 当加载完成并且准备激活新场景时，手动激活新场景
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            await Task.Yield(); // 等待一帧，避免阻塞主线程
        }
    }
}
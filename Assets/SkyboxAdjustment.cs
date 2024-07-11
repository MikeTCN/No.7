using UnityEngine;

public class SkyboxAdjustment : MonoBehaviour
{
    public Material skyboxMaterial; // 你的Skybox材質

    void Start()
    {
        // 設置Skybox材質的偏移量，使圖片合併的位置不再明顯
        skyboxMaterial.SetFloat("_Rotation", 180f); // 調整旋轉角度以適應你的圖片
    }
}
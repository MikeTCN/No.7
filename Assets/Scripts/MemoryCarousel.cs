using UnityEngine;

public class MemoryCarousel : MonoBehaviour
{
    public GameObject[] photoFrames;
    public float radius = 5f;
    public float rotationSpeed = 10f;
    public Transform playerCamera;
    public float photoTiltAngle = 10f;
    public float curveAmount = 1f; // 新增：控制向內彎曲的程度

    private float currentAngle = 0f;

    void Start()
    {
        PositionPhotos();
    }

    void Update()
    {
        RotatePhotos();
        FacePhotosToPlayer();
    }

    void PositionPhotos()
    {
        int photoCount = photoFrames.Length;
        float angleStep = 360f / photoCount;

        for (int i = 0; i < photoCount; i++)
        {
            float angle = i * angleStep;
            Vector3 position = CalculatePhotoPosition(angle);
            photoFrames[i].transform.position = position;
        }
    }

    Vector3 CalculatePhotoPosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians) * radius;
        float z = Mathf.Cos(radians) * radius;

        // 應用向內彎曲
        Vector3 basePosition = new Vector3(x, 0, z);
        Vector3 curveOffset = -basePosition.normalized * curveAmount;

        return transform.position + basePosition + curveOffset;
    }

    void RotatePhotos()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        int photoCount = photoFrames.Length;
        float angleStep = 360f / photoCount;

        for (int i = 0; i < photoCount; i++)
        {
            float angle = currentAngle + i * angleStep;
            Vector3 position = CalculatePhotoPosition(angle);
            photoFrames[i].transform.position = position;
        }
    }

    void FacePhotosToPlayer()
    {
        foreach (GameObject photo in photoFrames)
        {
            Vector3 directionToCamera = playerCamera.position - photo.transform.position;
            directionToCamera.y = 0; // 保持垂直方向不變

            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
            Quaternion tiltRotation = Quaternion.Euler(photoTiltAngle, 0, 0);

            photo.transform.rotation = lookRotation * tiltRotation;
        }
    }
}
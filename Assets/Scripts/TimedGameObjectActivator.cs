using UnityEngine;
using System;
using System.Collections.Generic;

public class TimedGameObjectActivator : MonoBehaviour
{
    [Serializable]
    public class TimedObject
    {
        public float activationTime;
        public GameObject objectToActivate;
    }

    public List<TimedObject> timedObjects = new List<TimedObject>();

    private float elapsedTime = 0f;
    private int currentIndex = 0;

    private void Update()
    {
        if (currentIndex >= timedObjects.Count)
            return;

        elapsedTime += Time.deltaTime;

        while (currentIndex < timedObjects.Count && elapsedTime >= timedObjects[currentIndex].activationTime)
        {
            ActivateObject(timedObjects[currentIndex]);
            currentIndex++;
        }
    }

    private void ActivateObject(TimedObject timedObject)
    {
        if (timedObject.objectToActivate != null)
        {
            timedObject.objectToActivate.SetActive(true);
            Debug.Log($"Activated {timedObject.objectToActivate.name} at {elapsedTime} seconds ⏰🎭");
        }
        else
        {
            Debug.LogWarning($"Object at index {currentIndex} is null! ⚠️");
        }
    }

    // 用於重置計時器的公共方法
    public void ResetTimer()
    {
        elapsedTime = 0f;
        currentIndex = 0;
        Debug.Log("Timer reset! ⏱️🔄");
    }
}
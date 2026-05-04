using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int animalsToCatch = 1;
    private int currentAnimals = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple GameManager instances found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AnimalCaptured()
    {
        currentAnimals++;

        Debug.Log($"Animals: {currentAnimals}/{animalsToCatch}");

        if (currentAnimals >= animalsToCatch)
        {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Level Complete!");
    }
}

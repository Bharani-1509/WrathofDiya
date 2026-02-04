using System.Collections;
using UnityEngine;

public class OrderPuzzleManager : MonoBehaviour
{
    [Header("Correct Order (orb IDs)")]
    public int[] correctOrder;

    [Header("Reward")]
    public GameObject rewardOrbPrefab;
    public Transform rewardSpawnPoint;

    [Header("Timing")]
    public float wrongFlashTime = 0.5f;

    private int currentIndex = 0;
    private bool puzzleSolved = false;
    private bool resetting = false;

    private PuzzleOrb[] allOrbs;

    void Awake()
    {
        allOrbs = FindObjectsOfType<PuzzleOrb>();

        foreach (PuzzleOrb orb in allOrbs)
            orb.puzzleManager = this;
    }

    public void HitOrb(PuzzleOrb orb)
    {
        if (puzzleSolved) return;
        if (resetting) return;
        if (orb == null) return;

        if (correctOrder == null || correctOrder.Length == 0) return;
        if (currentIndex >= correctOrder.Length) return;

        // CORRECT
        if (orb.orbID == correctOrder[currentIndex])
        {
            orb.SetCorrectColor();
            orb.Lock();
            orb.HideOrb();

            currentIndex++;

            if (currentIndex >= correctOrder.Length)
                PuzzleSolved();
        }
        else
        {
            StartCoroutine(HandleWrongOrb(orb));
        }
    }

    IEnumerator HandleWrongOrb(PuzzleOrb orb)
    {
        resetting = true;

        if (orb != null)
            orb.SetWrongColor();

        yield return new WaitForSeconds(wrongFlashTime);

        ResetPuzzle();

        resetting = false;
    }

    void ResetPuzzle()
    {
        currentIndex = 0;

        foreach (PuzzleOrb orb in allOrbs)
            if (orb != null) orb.ResetOrb();
    }

    void PuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("✅ Puzzle Solved!");

        // Disable all orbs
        foreach (PuzzleOrb orb in allOrbs)
        {
            if (orb != null)
            {
                orb.DisableOrb();
                orb.HideOrb();
            }
        }

        // Spawn reward
        if (rewardOrbPrefab != null && rewardSpawnPoint != null)
        {
            Instantiate(rewardOrbPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
        }
        else
        {
            Debug.LogError("❌ Reward prefab or spawn point missing!");
        }
    }
}

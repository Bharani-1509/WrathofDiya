using UnityEngine;
using System.Collections;

public class OrderPuzzleManager : MonoBehaviour
{
    public int[] correctOrder;   // e.g. 1,2,3,4,5
    private int currentIndex = 0;

    public GameObject rewardOrbPrefab;
    public Transform rewardSpawnPoint;

    public float wrongFlashTime = 0.5f; // how long red shows

    public void HitOrb(PuzzleOrb orb)
    {
        // CORRECT orb
        if (orb.orbID == correctOrder[currentIndex])
        {
            orb.SetCorrectColor();
            currentIndex++;

            // finished all?
            if (currentIndex >= correctOrder.Length)
            {
                PuzzleSolved();
            }
        }
        else
        {
            // WRONG orb
            StartCoroutine(HandleWrongOrb(orb));
        }
    }

    IEnumerator HandleWrongOrb(PuzzleOrb orb)
    {
        orb.SetWrongColor();
        Debug.Log("❌ Wrong order – flashing red");

        // Wait so player sees red
        yield return new WaitForSeconds(wrongFlashTime);

        ResetPuzzle();
    }

    void PuzzleSolved()
    {
        Debug.Log("✅ Puzzle Solved!");

        if (rewardOrbPrefab != null && rewardSpawnPoint != null)
        {
            Instantiate(
                rewardOrbPrefab,
                rewardSpawnPoint.position,
                rewardSpawnPoint.rotation
            );
        }
    }

    void ResetPuzzle()
    {
        Debug.Log("Resetting puzzle...");

        currentIndex = 0;

        // Reset ALL orb colors
        PuzzleOrb[] allOrbs = FindObjectsOfType<PuzzleOrb>();
        foreach (PuzzleOrb o in allOrbs)
        {
            o.ResetColor();
        }
    }
}

// === PatternPuzzleManager.cs ===
using System.Collections.Generic;
using UnityEngine;

public class PatternPuzzleManager : MonoBehaviour
{
    [Header("Solution sequence (fill in Inspector)")]
    public List<int> solutionPattern = new List<int>();

    [Header("Reward on success")]
    public GameObject rewardPrefab;
    public Transform rewardSpawnPoint;

    private int currentIndex = 0;
    private List<PatternPuzzleNode> usedNodes = new List<PatternPuzzleNode>();
    private bool isSolved = false;

    public bool TryNode(PatternPuzzleNode node)
    {
        if (isSolved) return false;
        if (node == null) return false;

        int expected = currentIndex < solutionPattern.Count ? solutionPattern[currentIndex] : -1;

        Debug.Log($"[Puzzle] Expected = {expected}   Shot = {node.nodeID}");

        if (node.nodeID != expected)
        {
            Debug.LogWarning("[Puzzle] Wrong node → resetting puzzle");
            ResetPuzzle();
            return false;
        }

        Debug.Log("[Puzzle] Correct node");

        usedNodes.Add(node);
        currentIndex++;

        if (currentIndex >= solutionPattern.Count)
        {
            isSolved = true;
            OnPuzzleSolved();
        }

        return true;
    }

    public void ResetPuzzle()
    {
        Debug.Log("[Puzzle] Resetting entire puzzle");

        currentIndex = 0;

        foreach (var node in usedNodes)
        {
            if (node != null) node.ResetVisualAndState();
        }

        usedNodes.Clear();
    }

    private void OnPuzzleSolved()
    {
        Debug.Log("═══════════════════════");
        Debug.Log("     PUZZLE SOLVED!    ");
        Debug.Log("═══════════════════════");

        if (rewardPrefab != null && rewardSpawnPoint != null)
        {
            Instantiate(rewardPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
            Debug.Log("Reward object instantiated");
        }
        else
        {
            Debug.LogWarning("Reward not spawned – missing prefab or spawn point");
        }

        // Optional: disable nodes / puzzle after solve
        // foreach (var node in FindObjectsOfType<PatternPuzzleNode>()) node.enabled = false;
    }

    [ContextMenu("Force Reset (Editor)")]
    void EditorForceReset() => ResetPuzzle();
}
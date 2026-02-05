using System.Collections.Generic;
using UnityEngine;

public class PatternPuzzleManager : MonoBehaviour
{
    [Header("Solution sequence")]
    public List<int> solutionPattern = new List<int>();

    [Header("Reward Settings")]
    public GameObject rewardObject;     // ← drag your reward object/prefab here

    private int currentIndex = 0;
    private bool isSolved = false;
    private List<PatternPuzzleNode> usedNodes = new List<PatternPuzzleNode>();

    void Start()
    {
        if (rewardObject != null)
            rewardObject.SetActive(false);
    }

    public bool TryNode(PatternPuzzleNode node)
    {
        if (isSolved || node == null) return false;

        if (node.nodeID != solutionPattern[currentIndex])
        {
            ResetPuzzle();
            return false;
        }

        usedNodes.Add(node);
        currentIndex++;

        if (currentIndex >= solutionPattern.Count)
        {
            OnPuzzleSolved();
        }

        return true;
    }

    void ResetPuzzle()
    {
        currentIndex = 0;
        foreach (var n in usedNodes)
            if (n != null) n.ResetVisualAndState();
        usedNodes.Clear();
    }

    void OnPuzzleSolved()
    {
        isSolved = true;
        Debug.Log("Puzzle solved → reward is now visible");

        if (rewardObject != null)
            rewardObject.SetActive(true);
        else
            Debug.LogWarning("Reward object not assigned in inspector");
    }
}
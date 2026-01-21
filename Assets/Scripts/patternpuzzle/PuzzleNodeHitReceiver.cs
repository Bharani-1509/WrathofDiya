// === PuzzleNodeHitReceiver.cs ===
// (remains almost unchanged – just better naming & safety)
using UnityEngine;

public class PuzzleNodeHitReceiver : MonoBehaviour
{
    private PatternPuzzleNode targetNode;

    void Awake()
    {
        targetNode = GetComponent<PatternPuzzleNode>();
        if (targetNode == null)
            targetNode = GetComponentInParent<PatternPuzzleNode>();

        if (targetNode == null)
        {
            Debug.LogError($"No PatternPuzzleNode component found on {gameObject.name} or its parents", this);
            Destroy(this);
        }
    }

    public void OnShot()    // ← called by your gun via SendMessage
    {
        if (targetNode != null)
            targetNode.OnShot();
    }
}
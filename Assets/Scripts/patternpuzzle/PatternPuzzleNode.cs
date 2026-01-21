// === PatternPuzzleNode.cs ===
using UnityEngine;

public class PatternPuzzleNode : MonoBehaviour
{
    [Header("Must be unique and exist in manager's solution list")]
    public int nodeID = -1;

    [Header("Materials – all 3 should be assigned")]
    public Material defaultMat;
    public Material correctMat;
    public Material wrongMat;

    private bool isUsed = false;
    private Renderer rend;
    private PatternPuzzleManager manager;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"Node {gameObject.name} is missing Renderer component!", this);
            enabled = false;
            return;
        }

        manager = FindObjectOfType<PatternPuzzleManager>(true);
        if (manager == null)
        {
            Debug.LogError("PatternPuzzleManager not found in scene!", this);
            enabled = false;
            return;
        }

        if (defaultMat == null) Debug.LogError($"Node {nodeID} ({name}) → defaultMat is not assigned!", this);
        if (correctMat == null) Debug.LogWarning($"Node {nodeID} ({name}) → correctMat missing");
        if (wrongMat == null) Debug.LogWarning($"Node {nodeID} ({name}) → wrongMat missing");

        ResetVisualAndState();
    }

    public void OnShot()
    {
        if (isUsed)
        {
            Debug.Log($"[Node {nodeID}] already used – ignoring shot");
            return;
        }

        isUsed = true;

        bool success = manager.TryNode(this);

        if (success)
        {
            if (correctMat != null)
                rend.material = correctMat;
            Debug.Log($"[Node {nodeID}] → correct (green)");
        }
        else
        {
            if (wrongMat != null)
                rend.material = wrongMat;
            Debug.Log($"[Node {nodeID}] → wrong (red) → will reset in 0.6s");

            // Give player time to notice wrong choice
            Invoke(nameof(ResetVisualAndState), 0.6f);
        }
    }

    public void ResetVisualAndState()
    {
        isUsed = false;

        if (rend != null)
        {
            if (defaultMat != null)
            {
                rend.material = defaultMat;
                Debug.Log($"[Node {nodeID}] → reset to default");
            }
            else
            {
                Debug.LogError($"[Node {nodeID}] cannot reset – defaultMat is null", this);
                // Very visible fallback so you notice the issue
                rend.material.color = Color.magenta;
            }
        }
    }

    // Editor helpers (right-click component in Inspector)
    [ContextMenu("Test → Set Correct")]
    void EditorSetCorrect() => rend.material = correctMat;

    [ContextMenu("Test → Set Wrong")]
    void EditorSetWrong() => rend.material = wrongMat;

    [ContextMenu("Test → Reset to Default")]
    void EditorReset() => ResetVisualAndState();
}
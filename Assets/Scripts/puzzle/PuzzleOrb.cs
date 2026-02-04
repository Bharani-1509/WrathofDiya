using UnityEngine;

public class PuzzleOrb : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int orbID;
    public OrderPuzzleManager puzzleManager;

    [Header("Materials")]
    public Material normalMat;
    public Material correctMat;
    public Material wrongMat;

    private Renderer[] allRenderers;
    private Collider[] allColliders;

    public bool IsLocked { get; private set; } = false;
    private bool isDisabled = false;

    void Awake()
    {
        // Get EVERYTHING under this orb (parent + child spheres + effects etc.)
        allRenderers = GetComponentsInChildren<Renderer>(true);
        allColliders = GetComponentsInChildren<Collider>(true);

        if (allRenderers.Length == 0)
            Debug.LogError($"❌ No Renderers found under orb root: {gameObject.name}");

        if (allColliders.Length == 0)
            Debug.LogError($"❌ No Colliders found under orb root: {gameObject.name}");
    }

    void Start()
    {
        ResetOrb();
    }

    public void OnShot()
    {
        if (isDisabled) return;
        if (IsLocked) return;

        if (puzzleManager != null)
            puzzleManager.HitOrb(this);
    }

    public void Lock() => IsLocked = true;
    public void DisableOrb() => isDisabled = true;

    public void SetCorrectColor()
    {
        if (correctMat == null) return;

        foreach (var r in allRenderers)
            r.material = correctMat;
    }

    public void SetWrongColor()
    {
        if (wrongMat == null) return;

        foreach (var r in allRenderers)
            r.material = wrongMat;
    }

    public void SetNormalColor()
    {
        if (normalMat == null) return;

        foreach (var r in allRenderers)
            r.material = normalMat;
    }

    // Hides ENTIRE orb (parent + all children)
    public void HideOrb()
    {
        foreach (var r in allRenderers)
            r.enabled = false;

        foreach (var c in allColliders)
            c.enabled = false;
    }

    public void ShowOrb()
    {
        foreach (var r in allRenderers)
            r.enabled = true;

        foreach (var c in allColliders)
            c.enabled = true;
    }

    public void ResetOrb()
    {
        IsLocked = false;
        isDisabled = false;

        ShowOrb();
        SetNormalColor();
    }
}

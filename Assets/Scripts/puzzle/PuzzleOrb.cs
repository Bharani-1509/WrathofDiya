using UnityEngine;

public class PuzzleOrb : MonoBehaviour
{
    public int orbID;
    public OrderPuzzleManager puzzleManager;

    [Header("Materials")]
    public Material normalMat;
    public Material correctMat;   // BLUE
    public Material wrongMat;     // RED

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        ResetColor();
    }

    // Called when shot
    public void OnShot()
    {
        if (puzzleManager != null)
            puzzleManager.HitOrb(this);
    }

    public void SetCorrectColor()
    {
        if (rend != null && correctMat != null)
            rend.material = correctMat;
    }

    public void SetWrongColor()
    {
        if (rend != null && wrongMat != null)
            rend.material = wrongMat;
    }

    public void ResetColor()
    {
        if (rend != null && normalMat != null)
            rend.material = normalMat;
    }
}

using UnityEngine;

public class RaycastReceiver : MonoBehaviour
{
    private PuzzleOrb orb;

    void Awake()
    {
        // Finds PuzzleOrb on parent root
        orb = GetComponentInParent<PuzzleOrb>();
    }

    void OnMouseDown()
    {
        if (orb != null)
            orb.OnShot();
    }
}

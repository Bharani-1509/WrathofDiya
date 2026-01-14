using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Draw");
        }
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Attack");
        }
    }
}

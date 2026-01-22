using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Space;
    public KeyCode digKey = KeyCode.E;

    public float actionLockTime = 0.35f;

    private Animator anim;
    public bool IsBusy { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsBusy) return;

        if (Input.GetKeyDown(attackKey))
            StartCoroutine(DoAttack());

        if (Input.GetKeyDown(digKey))
            StartCoroutine(DoDig());
    }

    IEnumerator DoAttack()
    {
        IsBusy = true;
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(actionLockTime);
        IsBusy = false;
    }

    IEnumerator DoDig()
    {
        IsBusy = true;
        anim.ResetTrigger("Dig");
        anim.SetTrigger("Dig");
        yield return new WaitForSeconds(actionLockTime);
        IsBusy = false;
    }
}

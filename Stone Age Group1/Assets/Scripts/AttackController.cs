using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public bool IsHit { get; private set; }
 
    void Start()
    {
        
    }

    /// <summary>
    /// Можно ли произвести удар
    /// true - можно
    /// false - уже происходит удар
    /// </summary>
    /// <returns></returns>
    public bool Hit()
    {
        if (!IsHit)
        {
            IsHit = true;
            StartCoroutine("Attack");
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2);
        if (IsHit)
        {
            IsHit = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsHit)
            return;

        // Проверяем по какому обьекту производим удар
        switch (collision.gameObject.tag)
        {
            case "Tree":
                {
                    TreeController controller = collision.gameObject.GetComponent<TreeController>();
                    if(controller != null)
                    {
                        controller.Drop();
                    }
                }
                break;
            case "Kokonut":
                {
                    Destroy(collision.gameObject);
                }
                break;
        }
    }
}

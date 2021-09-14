using System.Collections;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] GameObject[] FruitsToDrop;
    [SerializeField] Vector2[] DropDirections;
    [SerializeField] float Power;

    [SerializeField] ParticleSystem particle;

    private bool IsDropping;

    // Start is called before the first frame update
    void Start()
    {
        particle = this.GetComponentInChildren<ParticleSystem>();    
    }

    public void Drop()
    {
        if (IsDropping)
        {
            return;
        }

        StartCoroutine("DropFruit");
    }

    IEnumerator DropFruit()
    {
        IsDropping = true;

        particle.Play();

        var fruitToDrop = FruitsToDrop[Random.Range(0, FruitsToDrop.Length)];
        var direction = DropDirections[Random.Range(0, DropDirections.Length)];

        var fruit = GameObject.Instantiate(fruitToDrop, this.transform.position, Quaternion.identity, this.transform.parent);
        var tag = fruit.tag;
        fruit.tag = "Untagged";
        fruit.SetActive(true);

        fruit.GetComponent<Rigidbody2D>().AddRelativeForce(direction * Power);
        yield return new WaitForSeconds(2);
        fruit.tag = tag;

        IsDropping = false;
    }
}

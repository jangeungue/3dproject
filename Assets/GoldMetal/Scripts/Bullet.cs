using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision) //�Ѿ� isTrigger�� �ÿ��� �ٲٱ�
    {
        if (!isRock && collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject, 3);
        }
    }


}

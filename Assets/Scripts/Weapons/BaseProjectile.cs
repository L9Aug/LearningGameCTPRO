using UnityEngine;
using System.Collections;

public class BaseProjectile : MonoBehaviour {

    protected float ProjectileDamage;

    public void SetUpProjectile(float Speed, float Damage)
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed, ForceMode.VelocityChange);
        ProjectileDamage = Damage;
        StartCoroutine(Lifetimer(10));
    }

    IEnumerator Lifetimer(float LifeSpan)
    {
        float TimeLeft = LifeSpan;

        while(TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.collider.SendMessage("Hit", ProjectileDamage, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }
}

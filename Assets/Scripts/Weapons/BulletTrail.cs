using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {

	public void SetupBulletTrail(Vector3 StartPos, Vector3 EndPos, float DisplayTime)
    {
        LineRenderer lr = GetComponent<LineRenderer>();

        lr.SetVertexCount(2);

        lr.SetPositions(new Vector3[] { StartPos, EndPos });

        StartCoroutine(LifeTime(DisplayTime));
    }

    IEnumerator LifeTime(float lifeTime)
    {
        float TimeLeft = lifeTime;

        while(TimeLeft > 0)
        {
            yield return null;
            TimeLeft -= Time.deltaTime;
        }

        Destroy(gameObject);
    }
}

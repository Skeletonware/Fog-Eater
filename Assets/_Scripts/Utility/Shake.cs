using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake {

    public IEnumerator ShakeObject(GameObject obj, int amt, float shakeSize = 10, bool fade = true)
    {        
        float offX = UnityEngine.Random.Range(-shakeSize, shakeSize) / 100;
        float offY = UnityEngine.Random.Range(-shakeSize, shakeSize) / 100;

        Vector3 origPos = obj.transform.position;

        for (int i = 0; i < amt; i++)
        {
            offX = UnityEngine.Random.Range(-shakeSize, shakeSize) / 100;
            offY = UnityEngine.Random.Range(-shakeSize, shakeSize) / 100;
            if (fade)
            {
                shakeSize *= 0.975f;
            }
            obj.transform.position = origPos + new Vector3(offX, offY);
            yield return new WaitForEndOfFrame();
        }

        obj.transform.position = origPos;
    }
}

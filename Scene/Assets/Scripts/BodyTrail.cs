using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrail : MonoBehaviour {

    private WeaponTrail bodyTrail;                      //身体拖尾
    private float t = 0.033f;
    private float tempT = 0;
    private float animationIncrement = 0.003f;

    void Start()
    {
        bodyTrail = GetComponent<WeaponTrail>();
    }

    void LateUpdate()
    {
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (bodyTrail.time > 0)
                {
                    bodyTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    bodyTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (bodyTrail.time > 0)
            {
                bodyTrail.UpdateTrail(Time.time, t);
            }
        }
    }
}

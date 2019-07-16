using UnityEngine;
using System.Collections;

public class HeadBobber : MonoBehaviour
{

    private float timer = 0.0f;
    public float bobbingSpeed = 0.06f;
    public float bobbingAmount = 0.05f;
    public float midpoint = 2.0f;
    public float runSpeedMultiplier = 7f;
    public float runAmountMultiplier = 2f;
    public float walkSpeedMultiplier = 5f;
    public float walkAmountMultiplier = 2f;
    public bool isGrounded = true;

    private float defBobbingAmount;
    private float defbobbingSpeed;

    void Start()
    {
        defBobbingAmount = bobbingAmount;
        defbobbingSpeed = bobbingSpeed;
    }
    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = 1;
        float vertical = 1;
        if (!isGrounded)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        Vector3 v3T = transform.localPosition;
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            v3T.y = midpoint + translateChange;
        }
        else
        {
            v3T.y = midpoint;
        }
        transform.localPosition = v3T;
    }

    public void RunningBobbing()
    {
        bobbingAmount = defBobbingAmount * runAmountMultiplier;
        bobbingSpeed = defbobbingSpeed * runSpeedMultiplier;
    }

    public void WalkingBobbing()
    {
        bobbingAmount = defBobbingAmount * walkAmountMultiplier;
        bobbingSpeed = defbobbingSpeed * walkSpeedMultiplier;
    }

    public void IdleBobbing()
    {
        bobbingSpeed = defbobbingSpeed;
        bobbingAmount = defBobbingAmount;
    }

}
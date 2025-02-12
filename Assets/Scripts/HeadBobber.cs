﻿using UnityEngine;
using System.Collections;

public class HeadBobber : MonoBehaviour
{

    private float timer = 0.0f;
    private float verticalTimer = 0.0f;

    public float bobbingSpeed = 0.06f;
    public float bobbingAmount = 0.05f;
    public float midpoint = 2.0f;

    // public float verticalBobbingAmount = 0.025f;
    public float verticalBobbingAmount = 0.035f;
    public float verticalBobbingSpeed = 0.035f;
    private float verticalMidpoint = 0f;
    public float runSpeedMultiplier = 7f;
    public float runAmountMultiplier = 1.5f;
    public float walkSpeedMultiplier = 5f;
    public float walkAmountMultiplier = 1f;
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
        float verticalWaveslice = 0.0f;

        float horizontal = 1;
        float vertical = 1;
        if (!isGrounded)
        {
            timer = 0.0f;
            verticalTimer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }

            verticalWaveslice = Mathf.Sin(verticalTimer);
            verticalTimer = verticalTimer + verticalBobbingSpeed;
            if (verticalTimer > Mathf.PI * 2)
            {
                verticalTimer = verticalTimer - (Mathf.PI * 2);
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

        if (verticalWaveslice != 0)
        {
            float VerticalTranslateChange = verticalWaveslice * verticalBobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            VerticalTranslateChange = totalAxes * VerticalTranslateChange;
            v3T.x = verticalMidpoint + VerticalTranslateChange;
        }
        else
        {
            v3T.x = verticalMidpoint;
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
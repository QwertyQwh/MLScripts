using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MonsterLeagueInput : SingletonBehavior<MonsterLeagueInput>
{
    [Header("Keys!")]
    public KeyCode inputKeyX = KeyCode.X;
    public KeyCode inputKeyO;
    public KeyCode inputKeySqr;
    public KeyCode inputKeyTri;
    public KeyCode inputKeyUp;
    public KeyCode inputKeyDown;
    public KeyCode inputKeyLeft;
    public KeyCode inputKeyRight;

    public Action xPressed;
    public Action oPressed;
    public Action sqrPressed;
    public Action triPressed;
    public Action upPressed;
    public Action downPressed;
    public Action leftPressed;
    public Action rightPressed;



    [Header("FMOD")]
    public EventReference XEvent ;
    public EventReference OEvent;
    public EventReference SquareEvent;
    public EventReference TriangleEvent;
    public EventReference UpEvent;
    public EventReference DownEvent;
    public EventReference LeftEvent;
    public EventReference RightEvent;

    public bool IsInputDisabled  { get; set; }

    protected override void Start()
    {
        base.Start();
        IsInputDisabled = false;
        if (!XEvent.IsNull)
            xPressed += () => FMODUnity.RuntimeManager.PlayOneShot(XEvent, transform.position);
        if (!OEvent.IsNull)
            oPressed += () => FMODUnity.RuntimeManager.PlayOneShot(OEvent, transform.position);
        if (!SquareEvent.IsNull)
            sqrPressed += () => FMODUnity.RuntimeManager.PlayOneShot(SquareEvent, transform.position);
        if (!TriangleEvent.IsNull)
            triPressed += () => FMODUnity.RuntimeManager.PlayOneShot(TriangleEvent, transform.position);
        if (!UpEvent.IsNull)
            upPressed += () => FMODUnity.RuntimeManager.PlayOneShot(UpEvent, transform.position);
        if (!DownEvent.IsNull)
            downPressed += () => FMODUnity.RuntimeManager.PlayOneShot(DownEvent, transform.position);
        if (!LeftEvent.IsNull)
            leftPressed += () => FMODUnity.RuntimeManager.PlayOneShot(LeftEvent, transform.position);
        if (!RightEvent.IsNull)
            rightPressed += () => FMODUnity.RuntimeManager.PlayOneShot(RightEvent, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputDisabled)
            return;
        if (Input.GetKeyDown(inputKeyX))
        {
            xPressed?.Invoke();
        }
        if (Input.GetKeyDown(inputKeyO))
            oPressed?.Invoke();
        if (Input.GetKeyDown(inputKeyTri))
            triPressed?.Invoke();
        if (Input.GetKeyDown(inputKeySqr))
            sqrPressed?.Invoke();
        if (Input.GetKeyDown(inputKeyUp))
            upPressed?.Invoke();
        if (Input.GetKeyDown(inputKeyDown))
            downPressed?.Invoke();
        if (Input.GetKeyDown(inputKeyLeft))
            leftPressed?.Invoke();
        if (Input.GetKeyDown(inputKeyRight))
            rightPressed?.Invoke();
    }


}

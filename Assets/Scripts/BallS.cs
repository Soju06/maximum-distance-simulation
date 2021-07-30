using System;
using System.Diagnostics;
using UnityEngine;

public class BallS : MonoBehaviour {
    public GameObject LineObject;
    public GameObject ParticleObject;

    public TimeSpan FlyngTime { get; private set; }
    public TimeSpan HitedTime { get; private set; }
    public Vector3 StartPos { get; private set; }
    public Vector3 HighPos { get; private set; }
    public Vector3 HitPos { get; private set; }
    public Vector3 EndedPos { get; private set; }

    GameObject PO;
    LineRenderer LineRenderer;
    Rigidbody Rigidbody;

    public float Speed = 40;
    public float Angle = 40;

    Stopwatch FlyStopwatch = new Stopwatch();
    Stopwatch HitedStopwatch = new Stopwatch();


    bool isShooted = false;

    void Start() {
        Rigidbody = GetComponent<Rigidbody>();
        PO = Instantiate(LineObject);
        LineRenderer = PO.GetComponent<LineRenderer>();
    }

    Vector3 lps;

    private void FixedUpdate() {
        if (isShooted) {
            var pos = transform.position;
            if(lps != default && Vector3.Distance(Rigidbody.velocity, new Vector3(0.04f, 0.04f, 0.04f)) <= 0.07f) {
                isShooted = false;
                HitedStopwatch.Stop();
                FlyStopwatch.Stop();
                HitedTime = HitedStopwatch.Elapsed;
                EndedPos = pos;
                CopyPaticle();
                Rigidbody.velocity = new Vector3();
            }
            if (lps != default && Vector3.Distance(lps, pos) <= 0.1f) return;
            if (HighPos.y < pos.y) HighPos = pos;
            lps = pos;
            LineRenderer.positionCount++;
            LineRenderer.SetPosition(LineRenderer.positionCount - 1, pos);
        }
    }

    public void Shoot() {
        const bool h = true;
        var v = Speed * Mathf.Cos(Angle * (Mathf.PI / 180));
        var asd = new Vector3(h ? v : 0, Speed * Mathf.Sin(Angle * (Mathf.PI / 180)), h ? 0 : v);
        StartPos = transform.position;
        (Rigidbody ??= GetComponent<Rigidbody>()).constraints = RigidbodyConstraints.None;
        Rigidbody.velocity = asd;
        FlyStopwatch.Start();
        isShooted = true;
        ced = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if(ced) {
            ced = false;
            return;
        }
        if (!isShooted) return;
        FlyngTime = FlyStopwatch.Elapsed;
        HitPos = transform.position;
        HitedStopwatch.Start();
        CopyPaticle();
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //isShooted = false;
    }

    void CopyPaticle() {
        Instantiate(ParticleObject, transform.position, transform.rotation);
    }

    bool ced = false;

    public void SetViewPaticle(bool f) {
        if (isShooted) return;
        ced = true;
        PO.SetActive(f);
        gameObject.SetActive(f);
    }
}

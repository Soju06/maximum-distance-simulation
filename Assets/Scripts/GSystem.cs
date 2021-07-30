using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GSystem : MonoBehaviour {
    public float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�    
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )
    public float moveSpeed = 4.0f; // �̵� �ӵ�

    public GameObject BallObject;
    public Camera Camera1;
    public Camera Camera2;
    public Text Text;
    public float Speed = 40;
    public float Resistance = 0.2f;
    public float RollingResistance = 0.2f;
    public float Weight = 10f;

    Transform BallTransform;

    void Start() {
        BallTransform = GetComponent<Transform>();
        Camera1.enabled = true;
        Camera2.enabled = false;
    }

    List<BallS> Balls;

    bool e = false;

    long captureC = 0;

    void Copying() {
        var pos = transform.position;
        var rot = transform.rotation;
        pos.y += BallTransform.localScale.z / 2;
        Text.text = @$"
{Resistance} : ���� ����
{RollingResistance} : ȸ�� ����
{Weight} : ��ü �߷�
{Speed} : �߻� �ӷ�";
        var balls = new List<BallS>();
        for (int i = 5; i < 90; i += 5) {
            var e = Instantiate(BallObject, pos, rot);
            var ball = e.GetComponent<BallS>();
            var rb = e.GetComponent<Rigidbody>();
            ball.Angle = i;
            ball.Speed = Speed;
            rb.drag = Resistance;
            rb.angularDrag = RollingResistance;
            balls.Add(ball);
            pos.z += 7;
        }

        for (int i = 0; i < balls.Count; i++) 
            balls[i].Shoot();
        Balls = balls;
    }

    bool v = true;

    void Update() {
        if (Input.GetMouseButtonDown(0) && !e) {
            e = true;
            Copying();
        }

        if(Input.GetKeyDown(KeyCode.Tab) && Balls != null) {
            v ^= true;
            for (int i = 0; i < Balls.Count; i++) {
                Balls[i].SetViewPaticle(v);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            Camera1.enabled ^= true;
            Camera2.enabled ^= true;
        }

        if (Input.GetKey(KeyCode.C)) {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            ScreenCapture.CaptureScreenshot(Path.Combine(Application.streamingAssetsPath, $"capture_d{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_i{captureC++}_r{Resistance}_rr{RollingResistance}_w{Weight}_s{Speed}_c{Balls.Count}.png"), 2);
        }

        if (Input.GetKeyDown(KeyCode.S) && Balls != null) {
            var s = $"���� ����,ȸ�� ����,�ӵ�,����,���� �ð�,������ �ð�,���� ����,���� �Ÿ�,������ �Ÿ�,���� �Ÿ�\n";
            for (int i = 0; i < Balls.Count; i++) {
                var b = Balls[i];
                s += $"{Resistance},{RollingResistance},{b.Speed},{b.Angle},{b.FlyngTime:c},{b.HitedTime:c},{b.HighPos.y},{b.StartPos.x},{b.HitPos.x},{b.EndedPos.x}\n";
            }
            Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, $"result_r{Resistance}_rr{RollingResistance}_w{Weight}_s{Speed}_c{Balls.Count}.csv"), s);
        }


    }

    string ToS(Vector3 s) => $"{s.x} {s.y} {s.z}";
}

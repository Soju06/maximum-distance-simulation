using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GSystem : MonoBehaviour {
    public float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    public float moveSpeed = 4.0f; // 이동 속도

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
{Resistance} : 공기 저항
{RollingResistance} : 회전 저항
{Weight} : 개체 중량
{Speed} : 발사 속력";
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
            var s = $"공기 저항,회전 저항,속도,각도,비행 시간,굴러간 시간,고점 높이,시작 거리,떨어진 거리,멈춘 거리\n";
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

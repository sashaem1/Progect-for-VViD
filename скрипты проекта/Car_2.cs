using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Car_2 : MonoBehaviour
{
    public WheelCollider[] frontCols;
    public Transform[] dataFront;
    public WheelCollider[] backCols;
    public Transform[] dataBack;
    public Transform centerOfMass;
    public Rigidbody rb;
    private float maxSpeed;
    public float speed1 = 300f;
    public float speed2 = 500f;
    public float speed3 = 700f;
    private float sideSpead = 30f;
    public float breakSpeed = 1500f;
    private float speed; 
    public Text text;
    public Text text2;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        
    }


    void FixedUpdate()
    {
        float vAxis = Input.GetAxis("Vertical");
        float hAxis = Input.GetAxis("Horizontal");
        bool brekeButton = Input.GetButton("Jump");
        bool Force = Input.GetKey("left shift");

        // рассчёт скорости и вывод её на экран
        speed = rb.velocity.magnitude*3f;
        text.text = Math.Truncate(speed).ToString();
        text2.text = Math.Truncate(speed).ToString();

        // "переключение передач"
        if(Force == true) maxSpeed = 1500f;
        else{
            if(speed<30) maxSpeed=speed1;
            if(speed>=30&&speed<60)maxSpeed=speed2;
            if(speed>=60) maxSpeed=speed3;
            }

        // движение
        // backCols[0].motorTorque = vAxis * maxSpeed;
        // backCols[1].motorTorque = vAxis * maxSpeed;
        frontCols[0].motorTorque = vAxis * maxSpeed;
        frontCols[1].motorTorque = vAxis * maxSpeed;
        
        // торможение\задний ход
        if(vAxis<0)
        {
            // backCols[0].brakeTorque = vAxis * maxSpeed*200;
            // backCols[1].brakeTorque = vAxis * maxSpeed*200;
            frontCols[0].brakeTorque = Mathf.Abs(frontCols[0].motorTorque) * breakSpeed*200;
            frontCols[1].brakeTorque = Mathf.Abs(frontCols[1].motorTorque) * breakSpeed*200;
            backCols[0].brakeTorque = Mathf.Abs(backCols[0].motorTorque) * breakSpeed*200;
            backCols[1].brakeTorque = Mathf.Abs(backCols[1].motorTorque) * breakSpeed*200;
        }
 
        // ручник
        if(brekeButton)
        {
            frontCols[0].brakeTorque = breakSpeed*200;
            frontCols[1].brakeTorque = breakSpeed*200;
            // frontCols[0].brakeTorque = Mathf.Abs(frontCols[0].motorTorque) * breakSpeed;
            // frontCols[1].brakeTorque = Mathf.Abs(frontCols[1].motorTorque) * breakSpeed;
            backCols[0].brakeTorque = breakSpeed*200;
            backCols[1].brakeTorque = breakSpeed*200;
        } else {
            frontCols[0].brakeTorque = 0;
            frontCols[1].brakeTorque = 0;
            backCols[0].brakeTorque = 0;
            backCols[1].brakeTorque = 0;
        }

        // поворот колёс
        frontCols [0].steerAngle = hAxis * sideSpead;
        frontCols [1].steerAngle = hAxis * sideSpead;

        // визуал колёс
        dataFront[0].Rotate(-frontCols[0].rpm * Time.deltaTime,0,0);
        dataFront[1].Rotate(frontCols[1].rpm * Time.deltaTime,0,0);
        dataBack[0].Rotate(-backCols[0].rpm * Time.deltaTime,0,0);
        dataBack[1].Rotate(backCols[1].rpm * Time.deltaTime,0,0);
        dataFront[0].localEulerAngles = new Vector3 (dataFront[0].localEulerAngles.x, hAxis * sideSpead, 0);
        dataFront[1].localEulerAngles = new Vector3 (dataFront[1].localEulerAngles.x, hAxis * sideSpead, 0);
        // dataFront[0].localEulerAngles = new Vector3 (dataFront[0].localEulerAngles.x, hAxis * sideSpead, dataFront[0].localEulerAngles.z);
        // dataFront[1].localEulerAngles = new Vector3 (dataFront[1].localEulerAngles.x, hAxis * sideSpead, dataFront[1].localEulerAngles.z);
    }
}

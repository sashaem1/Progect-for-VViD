using UnityEngine;
using System.Collections;

public class BotControl : MonoBehaviour {
	
	public WheelCollider WheelBackLeftCol;// колайдеры колёс
	public WheelCollider WheelBackRightCol;
	public WheelCollider WheelForwardLeftCol;
	public WheelCollider WheelForwardRightCol;
	
	public Transform WheelBackLeftTransform;// Объекты колёс
	public Transform WheelBackRightTransform;
	public Transform WheelForwardLeftTransform;
	public Transform WheelForwardRightTransform;
	
	public Transform CentrOfMass;// Центр тяжести авто
	
	public float maxAccel = 25;// Мощьность
	public float maxBrake = 50;// Сила торможения
	public float maxRotate = 30;// угол поворота колёс
	public float maxSpeed = 60;// Скорость
	
	private float StartRadiusBackLeft;// Запись радиуса всех колёс по отдельности
	private float StartRadiusBackRight;
	private float StartRadiusForwardLeft;
	private float StartRadiusForwardRight;
	
	private float StartSusDistBackLeft;// Запись высоты подвески всех колёс по отдельности
	private float StartSusDistBackRight;
	private float StartSusDistForwardLeft;
	private float StartSusDistForwardRight;
	
	private Vector3 wheelStartPosBackLeft;// Начальное положение колёс
	private Vector3 wheelStartPosBackRight;
	private Vector3 wheelStartPosForwardLeft;
	private Vector3 wheelStartPosForwardRight;
	
	private float WheelRotationBackLeft;
	private float WheelRotationBackRight;
	private float WheelRotationForwardLeft;
	private float WheelRotationForwardRight;

    public Transform Target;// Цель бота
	public float MaxDistance = 5;// Дистанция до цели при достижении которой он будет отдаляться от цели
	public float MaxAlienation = 4;// Дистанция отдаления
        public GameObject[] Targets;// Список целей бота 
	public float TimeTargetUpdate = 2;// Через какое время бот обновит цель
        private float TimerUpdateTarget;// Таймер проверки цели  
	private bool ModeAlienation;// если бот приблизился ближе чем MaxDistance, то true и он начинает отъезжать 
	
	void Start () {// Тут идёт запись данных колёс
		if(WheelBackLeftCol == null || WheelBackRightCol == null || WheelForwardLeftCol == null || WheelForwardRightCol == null){
			print("No Wheel Collider");// если нет какого-то из колайдеров
			return;
		}	
		if(WheelBackLeftTransform == null || WheelBackRightTransform == null || WheelForwardLeftTransform == null || WheelForwardRightTransform == null){
			print("No Transform");// Если нет мешей 
			return;
		}
		if(CentrOfMass == null){
			print("No Centr Of Mass");// Если нет мешей 
			return;
		}
		
		StartRadiusBackLeft = WheelBackLeftCol.radius;// Запись радиуса всех колёс по отдельности
		StartRadiusBackRight = WheelBackRightCol.radius;
		StartRadiusForwardLeft = WheelForwardLeftCol.radius;
		StartRadiusForwardRight = WheelForwardRightCol.radius;
		
		StartSusDistBackLeft = WheelBackLeftCol.suspensionDistance;// Запись высоты подвески всех колёс по отдельности
		StartSusDistBackRight = WheelBackRightCol.suspensionDistance;
		StartSusDistForwardLeft = WheelForwardLeftCol.suspensionDistance;
		StartSusDistForwardRight = WheelForwardRightCol.suspensionDistance;
		
		wheelStartPosBackLeft = WheelBackLeftTransform.localPosition;// Запись начального положение колёс
		wheelStartPosBackRight = WheelBackRightTransform.localPosition;
		wheelStartPosForwardLeft = WheelForwardLeftTransform.localPosition;
		wheelStartPosForwardRight = WheelForwardRightTransform.localPosition;
		
		GetComponent<Rigidbody>().centerOfMass = CentrOfMass.localPosition;// Задаём центр тяжести
	}
	
	void Update () {
        float accel = 0f;// эта переменная будет от 1 до -1 она для самого движения взад и вперёд
		float rotare = 0f;// эта переменная будет содержать угол поворота колёс с рамками maxRotate
		float angleAccel = 0f;// эта переменная будет содержать угол относительно синей плоскости(рис.)
		float angleRotate = 0f;// эта переменная будет содержать угол относительно красной плоскости(рис.)
		if(Target == null){// если переменная Target пуста
			Targets = GameObject.FindGameObjectsWithTag("Player");// 
			if(Targets == null && Targets.Length == 1)return;// если цель одна или их вовсе нет то останавливаем исполнение
			for(int T = 0; T < Targets.Length; T++){
				if(transform != Targets[T].transform){// Если цель не сам бот
					Target = Targets[T].transform;// Тогда задаём цель
				}
			}
		}else{// если переменная не пуста 
			TimerUpdateTarget += Time.deltaTime;// Заводим таймер
			if(TimerUpdateTarget >= TimeTargetUpdate){// если время обновлять цель, то
				Targets = GameObject.FindGameObjectsWithTag("Player");// обновляем список целей 
				for(int T = 0; T < Targets.Length; T++){
					if(Vector3.Distance(transform.position, Target.position) > Vector3.Distance(transform.position, Targets[T].transform.position) && transform != Targets[T].transform){// если до новой цели ближе чем до старой
						Target = Targets[T].transform;// задаём новую цель
					}
				}
				TimerUpdateTarget = 0;// обнуляем таймер
			}
		}
        if(Target != null){// Если есть цель	
			if(ModeAlienation == false){// Если не включена инверсия
				angleAccel = -(Vector3.Angle(Target.position - transform.position, transform.forward)-90);// Это угол между целью и ботом для определения угла относительно синей плоскости(рис.)
				accel = Mathf.Clamp(angleAccel,-1,1);// Задаём accel в промежутке от -1 до 1
				if(accel > 0){// если цель спереди
					angleRotate = -(Vector3.Angle(Target.position - transform.position, transform.right)-90);// Это угол между целью и ботом для определения угла относительно красной плоскости(рис.)
				}else{//ecли цель сзади
					angleRotate = Vector3.Angle(Target.position - transform.position, transform.right)-90;// Это угол между целью и ботом для определения угла относительно красной плоскости(рис.)
				}
                                rotare = Mathf.Clamp(angleRotate,-maxRotate,maxRotate);// Задаём rotare в промежутке от -maxRotate до maxRotate
				if(MaxDistance > Vector3.Distance(transform.position, Target.position)){// если MaxDistance больше дистанции до цели
					ModeAlienation = true;// включаем инверсию чтобы бот отъезжал
				}
			}else{// Если включена
				angleAccel = (Vector3.Angle(Target.position - transform.position, transform.forward)-90);// Это угол между целью и ботом для определения угла относительно синей плоскости(рис.) с инверсией
				accel = Mathf.Clamp(angleAccel,-1,1);// Задаём accel в промежутке от -1 до 1
				angleRotate = Vector3.Angle(Target.position - transform.position, transform.right)-90;// Это угол между целью и ботом для определения угла относительно красной плоскости(рис.) с инверсией
				rotare = Mathf.Clamp(angleRotate,-maxRotate,maxRotate);// Задаём rotare в промежутке от -maxRotate до maxRotate
				if(MaxDistance + MaxAlienation < Vector3.Distance(transform.position, Target.position)){// если бот достаточно отдалился
					ModeAlienation = false;// выключаем режим инверсии
				}
			}
		}
CarMove(accel,rotare);// Просчёт движения
		UpdateWheels();// Просчёт поведения колеса
	}
	
	private void CarMove(float accel,float rotare){// Вычисления управления	
		if(accel > 0){
			if(WheelBackLeftCol.rpm < 0) WheelBackLeftCol.brakeTorque = maxBrake;
			if(WheelBackRightCol.rpm < 0) WheelBackRightCol.brakeTorque = maxBrake;
			else{
				WheelBackLeftCol.brakeTorque = 0;
				WheelBackRightCol.brakeTorque = 0;
				if(WheelBackLeftCol.rpm > maxSpeed*10)WheelBackLeftCol.motorTorque = 0;
				else WheelBackLeftCol.motorTorque = accel*maxAccel;
			
				if(WheelBackRightCol.rpm > maxSpeed*10)WheelBackRightCol.motorTorque = 0;
				else WheelBackRightCol.motorTorque = accel*maxAccel;
			}
		}
		else if(accel < 0){
			if(WheelBackLeftCol.rpm > 0) WheelBackLeftCol.brakeTorque = maxBrake;
			if(WheelBackRightCol.rpm > 0) WheelBackRightCol.brakeTorque = maxBrake;
			else{
				WheelBackLeftCol.brakeTorque = 0;
				WheelBackRightCol.brakeTorque = 0;
				if(WheelBackLeftCol.rpm < -maxSpeed*10)WheelBackLeftCol.motorTorque = 0;
				else WheelBackLeftCol.motorTorque = accel*maxAccel;
			
				if(WheelBackRightCol.rpm < -maxSpeed*10)WheelBackRightCol.motorTorque = 0;
				else WheelBackRightCol.motorTorque = accel*maxAccel;
			}
		}
		
		WheelForwardLeftCol.steerAngle = rotare;
		WheelForwardRightCol.steerAngle = rotare;
	}
	
	private void UpdateWheels(){// Просчёт Поведения колеса
		// WheelHit hit;
		Vector3 lpBl = WheelBackLeftTransform.localPosition;
		Vector3 lpBr = WheelBackRightTransform.localPosition;
		Vector3 lpFl = WheelForwardLeftTransform.localPosition;
		Vector3 lpFr = WheelForwardRightTransform.localPosition;
		
		// if(WheelBackLeftCol.GetGroundHit(out hit))lpBl.y -= Vector3.Dot(WheelBackLeftTransform.position - hit.point, transform.up) - StartRadiusBackLeft;
		// else lpBl.y = wheelStartPosBackLeft.y - StartSusDistBackLeft;
		// if(WheelBackRightCol.GetGroundHit(out hit))lpBr.y -= Vector3.Dot(WheelBackRightTransform.position - hit.point, transform.up) - StartRadiusBackRight;
		// else lpBr.y = wheelStartPosBackRight.y - StartSusDistBackRight;
		// if(WheelForwardLeftCol.GetGroundHit(out hit))lpFl.y -= Vector3.Dot(WheelForwardLeftTransform.position - hit.point, transform.up) - StartRadiusForwardLeft;
		// else lpFl.y = wheelStartPosForwardLeft.y - StartSusDistForwardLeft;
		// if(WheelForwardRightCol.GetGroundHit(out hit))lpFr.y -= Vector3.Dot(WheelForwardRightTransform.position - hit.point, transform.up) - StartRadiusForwardRight;
		// else lpFr.y = wheelStartPosForwardRight.y - StartSusDistForwardRight;
		
		WheelBackLeftTransform.localPosition = lpBl;
		WheelBackRightTransform.localPosition = lpBr;
		WheelForwardLeftTransform.localPosition = lpFl;
		WheelForwardRightTransform.localPosition = lpFr;
		
		WheelRotationBackLeft = Mathf.Repeat(WheelRotationBackLeft + Time.fixedDeltaTime * WheelBackLeftCol.rpm * 360.0f / 60.0f, 360.0f);
		WheelRotationBackRight = Mathf.Repeat(WheelRotationBackRight + Time.fixedDeltaTime * WheelBackRightCol.rpm * 360.0f / 60.0f, 360.0f);
		WheelRotationForwardLeft = Mathf.Repeat(WheelRotationForwardLeft + Time.fixedDeltaTime * WheelForwardLeftCol.rpm * 360.0f / 60.0f, 360.0f);
		WheelRotationForwardRight = Mathf.Repeat(WheelRotationForwardRight + Time.fixedDeltaTime * WheelForwardRightCol.rpm * 360.0f / 60.0f, 360.0f);
		
		WheelBackLeftTransform.localRotation = Quaternion.Euler(WheelRotationBackLeft, WheelBackLeftCol.steerAngle, 180f);
		WheelBackRightTransform.localRotation = Quaternion.Euler(WheelRotationBackRight, WheelBackRightCol.steerAngle,0f);	
		WheelForwardLeftTransform.localRotation = Quaternion.Euler(-WheelRotationForwardLeft, WheelForwardLeftCol.steerAngle, 180f);
		WheelForwardRightTransform.localRotation = Quaternion.Euler(WheelRotationForwardRight, WheelForwardRightCol.steerAngle, 0f);
    }
}            	
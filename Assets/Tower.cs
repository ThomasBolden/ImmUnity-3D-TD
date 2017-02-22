using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	Transform turretTransform;

	public float range = 50f;
	public GameObject bulletPrefab;

	public int cost = 5;

	float fireCooldown = 0.5f;
	float fireCooldownLeft = 0;

	public float damage = 1;
	public float radius = 0;

	//// Use this for initialization
	void Start () {
		turretTransform = transform.Find("Cube");
	}
	
	//// Update is called once per frame
	void Update () {
		//// TODO: Optimize this!
		Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

		Enemy nearestEnemy = null;
		float dist = Mathf.Infinity;

		foreach(Enemy e in enemies) {
			float d = Vector3.Distance(this.transform.position, e.transform.position);
			if(nearestEnemy == null || d < dist) {
				nearestEnemy = e;
				dist = d;
			}
		}

		if(nearestEnemy == null) {
			Debug.Log("No enemies?");
			return;
		}

		Vector3 dir = nearestEnemy.transform.position - this.transform.position;

		Quaternion lookRot = Quaternion.LookRotation( dir );

		////Debug.Log(lookRot.eulerAngles.y);
		turretTransform.rotation = Quaternion.Euler( -90f, lookRot.eulerAngles.y, 0 );

		fireCooldownLeft -= Time.deltaTime;
		//Debug.Log("fireCooldown: ");
		//Debug.Log(fireCooldownLeft);
		//Debug.Log("dir.magnitude: ");
		//Debug.Log(dir.magnitude);
		if(fireCooldownLeft <= 0 && dir.magnitude <= range) {
			fireCooldownLeft = fireCooldown;
			ShootAt(nearestEnemy);
			//Debug.Log("Shooting?");
		}
		//else{
		//	Debug.Log("Not Shooting?");
		//}

	}

	void ShootAt(Enemy e) {
		//// TODO: Fire out the tip!
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);

		Bullet b = bulletGO.GetComponent<Bullet>();
		b.target = e.transform;
		b.damage = damage;
		b.radius = radius;
	}
}
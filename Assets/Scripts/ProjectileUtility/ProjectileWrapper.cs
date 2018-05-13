using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileWrapper : MonoBehaviour
{
	public Transform TargetTransform;
	
	private float initialAngle;
	private float duration;
	private float ratio;
	private Projectile projectile;
	private Vector2 initialPos2D;
	private bool isMoving;
	private List<Projectile.ProjectilePoint> projectilePoints;
	
	void Start ()
	{
		projectilePoints=new List<Projectile.ProjectilePoint>();
		this.initialPos2D = this.transform.position;

		this.initialAngle = 30;
		this.duration = 2;
		this.ratio = 0.5f;

		ConstructProjectile();
		TargetMovement.OnPositionChanged += ResetAnimation;
	}

	public void SetAngle(Slider slider)
	{
		this.initialAngle = slider.value;
		Debug.Log(this.initialAngle);
		ResetAnimation();
	}

	public void SetDuration(Slider slider)
	{
		this.duration = slider.value;
		ResetAnimation();
	}
	
	public void SetRatio(Slider slider)
	{
		this.ratio = slider.value/100f;
		ResetAnimation();
	}
	
	public void SetTargetY(Slider slider)
	{
		this.TargetTransform.position=new Vector3(0,slider.value,0);
		ResetAnimation();
	}
	
	public void SetTargetX(Slider slider)
	{
		this.TargetTransform.position=new Vector3(slider.value,0,0);
		ResetAnimation();
	}
	
	private void ConstructProjectile()
	{
		Projectile.InitialAngleDuration initialAngleDuration = new Projectile.InitialAngleDuration();
		initialAngleDuration.InitAngle = this.initialAngle;
		initialAngleDuration.Duration = this.duration;

		projectile = new Projectile(this.ratio, initialAngleDuration);
		projectilePoints = projectile.GetProjectileSamples(this.initialPos2D, this.TargetTransform.position);
	}

	void FixedUpdate () {
		if (!this.isMoving)
			StartCoroutine(Animate());
	}

	void Update()
	{
		
	}
	public void ResetAnimation()
	{
		this.isMoving=false;
		StopAllCoroutines();
		ConstructProjectile();
	}

	IEnumerator Animate()
	{
		this.isMoving = true;
		List<Vector2> pointsVec2=projectilePoints.Select(s=>s.Position2D).ToList();

		for (var i = 0; i < pointsVec2.Count; i++)
		{
			transform.position = pointsVec2[i];
			yield return null;
		}

		this.isMoving = false;
		this.transform.position = pointsVec2[0];
	}
	
	void OnDrawGizmos()
	{
		if (this.projectilePoints==null || this.projectilePoints.Count==0) return;

		for (int i = 0; i < this.projectilePoints.Count-1; i++)
		{
			if (projectilePoints[i].ProjectileId == 0)
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.magenta;
			
			if (i%2==0)
			Gizmos.DrawLine(this.projectilePoints[i].Position2D,this.projectilePoints[i+1].Position2D);
		}
		
		Gizmos.color=Color.blue;
		Gizmos.DrawLine(this.initialPos2D,this.TargetTransform.position);
	}
}
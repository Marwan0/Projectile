using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class ProjectileWrapper : MonoBehaviour
{
	public float InitialAngle;
	public float Duration;
	public Transform TargetTransform;

	private Projectile projectile;
	private Vector2 initialPos2D;
	List<Vector2> pointsVec2=new List<Vector2>();
	private bool isMoving;
	
	void Start ()
	{
		this.initialPos2D = this.transform.position;
		
		Projectile.InitialAngleDuration initialAngleDuration = new Projectile.InitialAngleDuration();
		initialAngleDuration.InitAngle = this.InitialAngle;
		initialAngleDuration.Duration = this.Duration;
		
		projectile = new Projectile(0.6f,initialAngleDuration);
		TargetMovement.OnPositionChanged += StopAnimation;
	}

	void FixedUpdate () {
		
		if (!this.isMoving)
		{
			List<Projectile.ProjectilePoint> projectilePoints = projectile.GetProjectileSamples(this.initialPos2D,this.TargetTransform.position);
			pointsVec2 = projectilePoints.Select(s => s.Position2D).ToList();
			StartCoroutine(Animate());
		}
	}

	private void StopAnimation()
	{
		this.isMoving=false;
		StopAllCoroutines();
	}

	IEnumerator Animate()
	{
		this.isMoving = true;

		for (int i = 0; i < this.pointsVec2.Count; i++)
		{
			this.transform.position = this.pointsVec2[i];
			yield return null;
		}
		this.isMoving = false;
		this.transform.position = this.pointsVec2[0];
	}
	
	void OnDrawGizmos()
	{
		for (int i = 0; i < this.pointsVec2.Count-1; i++)
		{
			if (i%2==0)
			Gizmos.DrawLine(this.pointsVec2[i],this.pointsVec2[i+1]);
		}
		
		Gizmos.color=Color.magenta;
		Gizmos.DrawLine(this.initialPos2D,this.TargetTransform.position);
	}
}

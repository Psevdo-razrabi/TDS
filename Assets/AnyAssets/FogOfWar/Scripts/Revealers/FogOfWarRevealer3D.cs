using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Profiling;
#endif

//partially derrived from the sebastian lague Field Of View script. 
//this version has many changes and optimizations specific to this tool

namespace FOW
{
	public class FogOfWarRevealer3D : FogOfWarRevealer
	{
		private bool _circleIsComplete;
		private float _stepAngleSize;
		Vector3 _expectedNextPoint;
		protected override void _CalculateLineOfSight()
		{

#if UNITY_EDITOR
			Profiler.BeginSample("Revealing Hiders");
#endif

			RevealHiders();

#if UNITY_EDITOR
			Profiler.EndSample();
			NumRayCasts = 0;
#endif


#if UNITY_EDITOR
			Profiler.BeginSample("Line Of Sight");
#endif
			int stepCount = Mathf.RoundToInt(ViewAngle * RaycastResolution);
			_stepAngleSize = ViewAngle / stepCount;
			//Debug.Log(stepCount);
			//Debug.Log(_stepAngleSize);

			ViewCastInfo oldViewCast = new ViewCastInfo();
			_circleIsComplete = Mathf.Approximately(ViewAngle, 360);
			float firstAng = ((-GetEuler() + 360 + 90) % 360) - (ViewAngle / 2);
			ViewCastInfo firstViewCast = ViewCast(firstAng);

			base.AddViewPoint(firstViewCast);
            //if (!_circleIsComplete)
            //    base.AddViewPoint(firstViewCast);

            float angleC = 180 - (AngleBetweenVector2(-Vector3.Cross(firstViewCast.normal, FogOfWarWorld.upVector), -firstViewCast.direction.normalized) + _stepAngleSize);
			float nextDist = (firstViewCast.dst * Mathf.Sin(Mathf.Deg2Rad * _stepAngleSize)) / Mathf.Sin(Mathf.Deg2Rad * angleC);
			_expectedNextPoint = firstViewCast.point + (-Vector3.Cross(firstViewCast.normal, FogOfWarWorld.upVector) * nextDist);

			oldViewCast = firstViewCast;
			for (int i = 1; i <= stepCount; i++)
			{
				float angle = firstViewCast.angle + _stepAngleSize * i;
				ViewCastInfo newViewCast = ViewCast(angle);

				determineEdge(oldViewCast, newViewCast);

				angleC = 180 - (Mathf.Abs(AngleBetweenVector2(-Vector3.Cross(newViewCast.normal, FogOfWarWorld.upVector), -newViewCast.direction.normalized)) + _stepAngleSize);
				nextDist = (newViewCast.dst * Mathf.Sin(Mathf.Deg2Rad * _stepAngleSize)) / Mathf.Sin(Mathf.Deg2Rad * angleC);
				_expectedNextPoint = newViewCast.point + (-Vector3.Cross(newViewCast.normal, FogOfWarWorld.upVector) * nextDist);

#if UNITY_EDITOR
				if (DebugMode)
				{
					Vector3 dir = DirFromAngle(angle, true);
					if (newViewCast.hit)
						Debug.DrawRay(GetEyePosition(), dir * (newViewCast.dst), Color.green);
					else
						Debug.DrawRay(GetEyePosition(), dir * (newViewCast.dst), Color.red);
					Debug.DrawLine(newViewCast.point, _expectedNextPoint + FogOfWarWorld.upVector * .1f, Random.ColorHSV());
				}
#endif

				oldViewCast = newViewCast;
			}

			if (_circleIsComplete)
			{
				//firstViewCast.angle = 360;
				//determineEdge(oldViewCast, firstViewCast);
				//if (NumberOfPoints == 0)
				//{
				//	base.AddViewPoint(new ViewCastInfo(false, -Vector3.right * ViewRadius + getEyePos(), ViewRadius, 180, -Vector3.right, -Vector3.right));
				//}
				//base.AddViewPoint(ViewPoints[0]);
				firstViewCast.angle += 360;
				determineEdge(oldViewCast, firstViewCast);
				base.AddViewPoint(ViewPoints[0]);
			}
			else
			{
				base.AddViewPoint(oldViewCast);
			}

			ApplyData();

#if UNITY_EDITOR
			if (LogNumRaycasts)
				//Debug.Log($"Number of raycasts this update: {NumRayCasts}");
			Profiler.EndSample();
#endif
		}
        float GetEuler()
        {
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ: return transform.eulerAngles.y;
                case FogOfWarWorld.GamePlane.XY: return transform.eulerAngles.z;
                case FogOfWarWorld.GamePlane.ZY: return transform.eulerAngles.x;
            }
            return transform.eulerAngles.y;
        }
		Vector3 GetEyePosition()
        {
			return transform.position + FogOfWarWorld.upVector * EyeOffset;
		}
		Vector3 hiderPosition;
		void RevealHiders()
		{
			FogOfWarHider hiderInQuestion;
			float distToHider;
			float heightDist = 0;
			Vector3 eyePos = GetEyePosition();
			float sightDist = ViewRadius;
			if (revealHidersInFadeOutZone && FogOfWarWorld.instance.usingBlur)
				sightDist += FogOfWarWorld.instance.softenDistance + AdditionalSoftenDistance;
			for (int i = 0; i < FogOfWarWorld.numHiders; i++)
			{
				hiderInQuestion = FogOfWarWorld.hiders[i];
				bool seen = false;
				Transform samplePoint;
				float minDistToHider = distBetweenVectors(hiderInQuestion.transform.position, eyePos) - hiderInQuestion.maxDistBetweenPoints;
				if (minDistToHider < unobscuredRadius || (minDistToHider < sightDist))
				{
					for (int j = 0; j < hiderInQuestion.samplePoints.Length; j++)
					{
						samplePoint = hiderInQuestion.samplePoints[j];

						distToHider = distBetweenVectors(samplePoint.position, eyePos);
						switch(FogOfWarWorld.instance.gamePlane)
                        {
							case FogOfWarWorld.GamePlane.XZ: heightDist = Mathf.Abs(eyePos.y - samplePoint.position.y); break;
							case FogOfWarWorld.GamePlane.XY: heightDist = Mathf.Abs(eyePos.z - samplePoint.position.z); break;
							case FogOfWarWorld.GamePlane.ZY: heightDist = Mathf.Abs(eyePos.x - samplePoint.position.x); break;
                        }
                        if ((distToHider < unobscuredRadius || (distToHider < sightDist && Mathf.Abs(AngleBetweenVector2(samplePoint.position - eyePos, getForward())) < ViewAngle / 2)) && heightDist < VisionHeight)
						{
							setHiderPosition(samplePoint.position);
							if (!Physics.Raycast(eyePos, hiderPosition - eyePos, distToHider, ObstacleMask))
							{
								seen = true;
								break;
							}
						}
					}
				}
				if (seen)
                {
					if (!hidersSeen.Contains(hiderInQuestion))
					{
						hidersSeen.Add(hiderInQuestion);
						hiderInQuestion.addSeer(this);
					}
				}
				else
                {
					if (hidersSeen.Contains(hiderInQuestion))
					{
						hidersSeen.Remove(hiderInQuestion);
						hiderInQuestion.removeSeer(this);
					}
				}
			}
		}
		void setHiderPosition(Vector3 point)
        {
			switch (FogOfWarWorld.instance.gamePlane)
			{
				case FogOfWarWorld.GamePlane.XZ:
					hiderPosition.x = point.x;
					hiderPosition.y = GetEyePosition().y;
					hiderPosition.z = point.z;
					break;
				case FogOfWarWorld.GamePlane.XY:
					hiderPosition.x = point.x;
					hiderPosition.y = point.y;
					hiderPosition.z = GetEyePosition().z;
					break;
				case FogOfWarWorld.GamePlane.ZY:
					hiderPosition.x = GetEyePosition().x;
					hiderPosition.y = point.y;
					hiderPosition.z = point.z;
					break;
			}
		}
        protected override bool _TestPoint(Vector3 point)
        {
			float sightDist = ViewRadius;
			if (revealHidersInFadeOutZone && FogOfWarWorld.instance.usingBlur)
				sightDist += FogOfWarWorld.instance.softenDistance + AdditionalSoftenDistance;

			float distToPoint = distBetweenVectors(point, GetEyePosition());
			if (distToPoint < unobscuredRadius || (distToPoint < sightDist && Mathf.Abs(AngleBetweenVector2(point - GetEyePosition(), getForward())) < ViewAngle / 2))
			{
				setHiderPosition(point);
				if (!Physics.Raycast(GetEyePosition(), hiderPosition - transform.position, distToPoint, ObstacleMask))
					return true;
			}
			return false;
		}

		Vector2 center = new Vector2();
		private void ApplyData()
		{
#if UNITY_EDITOR
			if (DebugMode)
				Random.InitState(1);
#endif

			for (int i = 0; i < NumberOfPoints; i++)
			{
				//Vector3 difference = viewPoints[i].point - transform.position;
				//float deg = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
				//deg = (deg + 360) % 360;
#if UNITY_EDITOR
				if (DebugMode)
				{
					//Debug.Log(deg);
					Debug.DrawRay(GetEyePosition(), (ViewPoints[i].point - GetEyePosition()) + Random.insideUnitSphere * DrawRayNoise, Color.blue);

					if (i != 0)
						Debug.DrawLine(ViewPoints[i].point, ViewPoints[i - 1].point, Color.yellow);
				}
#endif
				Radii[i] = ViewPoints[i].angle;
				AreHits[i] = ViewPoints[i].hit;
				Distances[i] = ViewPoints[i].dst;
				if (i == NumberOfPoints - 1 && _circleIsComplete)
				{
					Radii[i] += 360;
				}
			}

			float heightPos = 0;
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ:
                    center.x = GetEyePosition().x;
                    center.y = GetEyePosition().z;
					heightPos = GetEyePosition().y;
                    break;
                case FogOfWarWorld.GamePlane.XY:
                    center.x = GetEyePosition().x;
                    center.y = GetEyePosition().y;
					heightPos = GetEyePosition().z;
					break;
                case FogOfWarWorld.GamePlane.ZY:
                    center.x = GetEyePosition().z;
                    center.y = GetEyePosition().y;
					heightPos = GetEyePosition().x;
					break;
            }

			CircleStruct.circleOrigin = center;
			CircleStruct.numSegments = NumberOfPoints;
			CircleStruct.unobscuredRadius = unobscuredRadius;
			CircleStruct.circleHeight = heightPos;
			//CircleStruct.isComplete = _circleIsComplete ? 1 : 0;
			CircleStruct.circleRadius = ViewRadius;
			CircleStruct.circleFade = FogOfWarWorld.instance.softenDistance + AdditionalSoftenDistance;
			CircleStruct.visionHeight = VisionHeight;
			CircleStruct.heightFade = VisionHeightSoftenDistance;

			FogOfWarWorld.instance.UpdateCircle(FogOfWarID, CircleStruct, NumberOfPoints, Radii, Distances, AreHits);
		}



		bool greaterThanLastAngle;
		void determineEdge(ViewCastInfo oldViewCast, ViewCastInfo newViewCast, int iteration = 0)
		{
			if (oldViewCast.hit != newViewCast.hit)
			{
				if (iteration >= NumExtraIterations)
				{
					EdgeInfo farEdge = FindEdge(newViewCast, oldViewCast, true);
					EdgeInfo closeEdge = FindEdge(oldViewCast, newViewCast);
					greaterThanLastAngle = farEdge.maxViewCast.angle > closeEdge.maxViewCast.angle;
					if (newViewCast.dst < oldViewCast.dst)
					{
						if (Mathf.Abs(closeEdge.minViewCast.dst - ViewRadius) < .01f || Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
						{
							base.AddViewPoint(closeEdge.minViewCast);
							base.AddViewPoint(closeEdge.maxViewCast);
						}
						else
							greaterThanLastAngle = true;
						if (Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f && greaterThanLastAngle)
						{
							base.AddViewPoint(farEdge.maxViewCast);
							base.AddViewPoint(farEdge.minViewCast);
						}
						//if (Mathf.Abs(closeEdge.minViewCast.dst - viewRadius) < .01f || Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
						//{
						//	base.AddViewPoint(closeEdge.minViewCast);
						//	base.AddViewPoint(closeEdge.maxViewCast);
						//}
						//if (Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f)
						//{
						//	base.AddViewPoint(farEdge.maxViewCast);
						//	base.AddViewPoint(farEdge.minViewCast);
						//}
					}
					else
					{
						if (Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
						{
							base.AddViewPoint(closeEdge.minViewCast);
							base.AddViewPoint(closeEdge.maxViewCast);
						}
						else
							greaterThanLastAngle = true;
						if ((Mathf.Abs(farEdge.maxViewCast.dst - ViewRadius) < .01f || Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f) && greaterThanLastAngle)
						{
							base.AddViewPoint(farEdge.maxViewCast);
							base.AddViewPoint(farEdge.minViewCast);
						}
						//if (Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
						//{
						//	base.AddViewPoint(closeEdge.minViewCast);
						//	base.AddViewPoint(closeEdge.maxViewCast);
						//}
						//if (Mathf.Abs(farEdge.maxViewCast.dst - viewRadius) < .01f || Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f)
						//{
						//	base.AddViewPoint(farEdge.maxViewCast);
						//	base.AddViewPoint(farEdge.minViewCast);
						//}
					}
				}
				else
				{
					castExtraRays(oldViewCast.angle, newViewCast.angle, oldViewCast, iteration + 1);
				}
			}
			else if (newViewCast.hit && oldViewCast.hit)
			{
				float ExpectedDelta = Vector3.Distance(_expectedNextPoint, newViewCast.point);
				if (ExpectedDelta > DoubleHitMaxDelta || Mathf.Abs(AngleBetweenVector2(newViewCast.normal, oldViewCast.normal)) > DoubleHitMaxAngleDelta)
				{
					if (iteration >= NumExtraIterations)
					{
						bool noneAdded = true;
						if (Vector3.Distance(newViewCast.point, oldViewCast.point) > DoubleHitMaxDelta)
						{
							EdgeInfo farEdge = FindEdge(newViewCast, oldViewCast, true);
							EdgeInfo closeEdge = FindEdge(oldViewCast, newViewCast);
							greaterThanLastAngle = farEdge.maxViewCast.angle > closeEdge.maxViewCast.angle;
							if (newViewCast.dst < oldViewCast.dst)
							{
								if (Mathf.Abs(closeEdge.minViewCast.dst - ViewRadius) < .01f || Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
								{
									base.AddViewPoint(closeEdge.minViewCast);
									base.AddViewPoint(closeEdge.maxViewCast);
									noneAdded = false;
								}
								else
									greaterThanLastAngle = true;
								if (Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f && greaterThanLastAngle)
								{
									base.AddViewPoint(farEdge.maxViewCast);
									base.AddViewPoint(farEdge.minViewCast);
									noneAdded = false;
								}
							}
							else
							{
								if (Mathf.Abs(closeEdge.minViewCast.dst - closeEdge.maxViewCast.dst) > .01f)
								{
									base.AddViewPoint(closeEdge.minViewCast);
									base.AddViewPoint(closeEdge.maxViewCast);
									noneAdded = false;
								}
								else
									greaterThanLastAngle = true;
								if ((Mathf.Abs(farEdge.maxViewCast.dst - ViewRadius) < .01f || Mathf.Abs(farEdge.minViewCast.dst - farEdge.maxViewCast.dst) > .01f) && greaterThanLastAngle)
								{
									base.AddViewPoint(farEdge.maxViewCast);
									base.AddViewPoint(farEdge.minViewCast);
									noneAdded = false;
								}
							}
						}
						if (noneAdded)
						{
							float deltaAngle = AngleBetweenVector2(newViewCast.normal, oldViewCast.normal);
							if (deltaAngle < 0)
							{
								EdgeInfo edge = FindMax(newViewCast, oldViewCast);
								base.AddViewPoint(edge.maxViewCast);
							}
							else if (addCorners && deltaAngle > 0)
							{
								EdgeInfo edge = FindMax(newViewCast, oldViewCast);
								base.AddViewPoint(edge.maxViewCast);
							}
						}

					}
					else
					{
						castExtraRays(oldViewCast.angle, newViewCast.angle, oldViewCast, iteration + 1);
					}
				}

			}
		}

		void castExtraRays(float minAngle, float maxAngle, ViewCastInfo oldViewCast, int iteration)
		{
			float newAngleChange = (maxAngle - minAngle) / NumExtraRaysOnIteration;

			float angleC = 180 - (AngleBetweenVector2(-Vector3.Cross(oldViewCast.normal, FogOfWarWorld.upVector), -oldViewCast.direction.normalized) + newAngleChange);
			float nextDist = (oldViewCast.dst * Mathf.Sin(Mathf.Deg2Rad * newAngleChange)) / Mathf.Sin(Mathf.Deg2Rad * angleC);
			_expectedNextPoint = oldViewCast.point + (-Vector3.Cross(oldViewCast.normal, FogOfWarWorld.upVector) * nextDist);

			for (int i = 0; i < NumExtraRaysOnIteration + 1; i++)
			{
				float angle = minAngle + (newAngleChange * i);
				ViewCastInfo newViewCast = ViewCast(angle);

				determineEdge(oldViewCast, newViewCast, iteration);

				angleC = 180 - (Mathf.Abs(AngleBetweenVector2(-Vector3.Cross(newViewCast.normal, FogOfWarWorld.upVector), -newViewCast.direction.normalized)) + newAngleChange);
				nextDist = (newViewCast.dst * Mathf.Sin(Mathf.Deg2Rad * newAngleChange)) / Mathf.Sin(Mathf.Deg2Rad * angleC);
				_expectedNextPoint = newViewCast.point + (-Vector3.Cross(newViewCast.normal, FogOfWarWorld.upVector) * nextDist);

#if UNITY_EDITOR
				if (DebugMode && DrawExtraCastLines)
				{
					Vector3 dir = DirFromAngle(angle, true);
					if (newViewCast.hit)
						Debug.DrawRay(GetEyePosition(), dir * (newViewCast.dst), Color.green);
					else
						Debug.DrawRay(GetEyePosition(), dir * (newViewCast.dst), Color.red);
					Debug.DrawLine(newViewCast.point, _expectedNextPoint + FogOfWarWorld.upVector * (.1f / iteration), Random.ColorHSV());
				}
#endif

				oldViewCast = newViewCast;
			}
		}


		Vector2 vec1;
		Vector2 vec2;
		Vector2 vec1Rotated90;
		private float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2)
		{
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ:
                    vec1.x = _vec1.x;
                    vec1.y = _vec1.z;
                    vec2.x = _vec2.x;
                    vec2.y = _vec2.z;
                    break;
                case FogOfWarWorld.GamePlane.XY:
                    vec1.x = _vec1.x;
                    vec1.y = _vec1.y;
                    vec2.x = _vec2.x;
                    vec2.y = _vec2.y;
                    break;
                case FogOfWarWorld.GamePlane.ZY:
                    vec1.x = _vec1.z;
                    vec1.y = _vec1.y;
                    vec2.x = _vec2.z;
                    vec2.y = _vec2.y;
                    break;
            }
            
			//vec1 = vec1.normalized;
			//vec2 = vec2.normalized;
			vec1Rotated90.x = -vec1.y;
			vec1Rotated90.y = vec1.x;
			//Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
			float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
			return Vector2.Angle(vec1, vec2) * sign;
		}
		float distBetweenVectors(Vector3 _vec1, Vector3 _vec2)
		{
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ:
                    vec1.x = _vec1.x;
                    vec1.y = _vec1.z;
                    vec2.x = _vec2.x;
                    vec2.y = _vec2.z;
                    break;
                case FogOfWarWorld.GamePlane.XY:
                    vec1.x = _vec1.x;
                    vec1.y = _vec1.y;
                    vec2.x = _vec2.x;
                    vec2.y = _vec2.y;
                    break;
                case FogOfWarWorld.GamePlane.ZY:
                    vec1.x = _vec1.z;
                    vec1.y = _vec1.y;
                    vec2.x = _vec2.z;
                    vec2.y = _vec2.y;
                    break;
            }
            return Vector2.Distance(vec1, vec2);
		}

        Vector3 getForward()
        {
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ: return transform.forward;
                case FogOfWarWorld.GamePlane.XY: return new Vector3(-transform.up.x, transform.up.y, 0).normalized;
                //case FogOfWarWorld.GamePlane.XY: return -transform.right;
                case FogOfWarWorld.GamePlane.ZY: return transform.up;
            }
            return transform.forward;
        }

		EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, bool isReflect = false)
		{
			float minAngle = minViewCast.angle;
			float maxAngle = maxViewCast.angle;

			for (int i = 0; i < MaxEdgeResolveIterations; i++)
			{
				float angle = (minAngle + maxAngle) / 2;
#if UNITY_EDITOR
				if (DebugMode && DrawIteritiveLines)
				{
					Vector3 dir = DirFromAngle(angle, true);
					Debug.DrawRay(GetEyePosition(), dir * (ViewRadius + 2), Color.white);
				}
#endif
				ViewCastInfo newViewCast = ViewCast(angle);

				bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > EdgeDstThreshold;
				edgeDstThresholdExceeded = edgeDstThresholdExceeded || Mathf.Abs(AngleBetweenVector2(newViewCast.normal, minViewCast.normal)) > 0;

				if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
				{
					minViewCast = newViewCast;
					minAngle = angle;
				}
				else
				{
					maxViewCast = newViewCast;
					maxAngle = angle;
				}
				if (Mathf.Abs(maxAngle - minAngle) < MaxAcceptableEdgeAngleDifference)
				{
					break;
				}
			}

			return new EdgeInfo(minViewCast, maxViewCast, true);
			//return new EdgeInfo(minPoint, maxPoint);
		}
		EdgeInfo FindMax(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
		{
			float minAngle = minViewCast.angle;
			float maxAngle = maxViewCast.angle;

			for (int i = 0; i < MaxEdgeResolveIterations; i++)
			{
				float angle = (minAngle + maxAngle) / 2;
#if UNITY_EDITOR
				if (DebugMode && DrawIteritiveLines)
				{
					Vector3 dir = DirFromAngle(angle, true);
					Debug.DrawRay(GetEyePosition(), dir * (ViewRadius + 2), Color.white);
				}
#endif
				ViewCastInfo newViewCast = ViewCast(angle);

				bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > EdgeDstThreshold;
				edgeDstThresholdExceeded = edgeDstThresholdExceeded || Mathf.Abs(AngleBetweenVector2(newViewCast.normal, minViewCast.normal)) > 0;
				if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
				{
					minViewCast = newViewCast;
					minAngle = angle;
				}
				else
				{
					maxViewCast = newViewCast;
					maxAngle = angle;
				}
				if (Mathf.Abs(maxAngle - minAngle) < MaxAcceptableEdgeAngleDifference)
				{
					break;
				}
			}

			return new EdgeInfo(minViewCast, maxViewCast, true);
		}

		RaycastHit rayHit;
		ViewCastInfo ViewCast(float globalAngle)
		{
#if UNITY_EDITOR
			NumRayCasts++;
#endif
			Vector3 dir = DirFromAngle(globalAngle, true);

			float rayDist = ViewRadius;
			if (FogOfWarWorld.instance.usingBlur)
				rayDist += FogOfWarWorld.instance.softenDistance + AdditionalSoftenDistance;
			if (Physics.Raycast(GetEyePosition(), dir, out rayHit, rayDist, ObstacleMask))
			{
				return new ViewCastInfo(true, rayHit.point, rayHit.distance, globalAngle, rayHit.normal, dir);
			}
			else
			{
				return new ViewCastInfo(false, GetEyePosition() + dir * ViewRadius, ViewRadius, globalAngle, Vector3.zero, dir);
			}
		}

		Vector3 direction = Vector3.zero;
		public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
            switch (FogOfWarWorld.instance.gamePlane)
            {
                case FogOfWarWorld.GamePlane.XZ:
                    if (!angleIsGlobal)
                    {
                        angleInDegrees += transform.eulerAngles.y;
                    }
                    direction.x = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
                    direction.z = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
                    return direction;
                case FogOfWarWorld.GamePlane.XY:
                    if (!angleIsGlobal)
                    {
                        angleInDegrees += transform.eulerAngles.z;
                    }
                    direction.x = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
                    direction.y = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
                    return direction;
                case FogOfWarWorld.GamePlane.ZY: break;
            }
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.x;
            }
            direction.z = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
            direction.y = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
            return direction;
        }		
	}
}
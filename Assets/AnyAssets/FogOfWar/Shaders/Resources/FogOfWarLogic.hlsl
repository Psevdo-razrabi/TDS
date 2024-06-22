#ifndef _CONESETUP_
#define _CONESETUP_

struct circleStruct
{
    float2 circleOrigin;
    int startIndex;
    int numSegments;
    float circleHeight;
    float unobscuredRadius;
    //bool isComplete;
    half circleRadius;
    half circleFade;
    half visionHeight;
    half heightFade;
};
struct ConeEdgeStruct
{
    float edgeAngle;
    float length;
    bool cutShort;
};

float _extraRadius;

float _fadeOutDegrees;
//float _fadeOutDistance;
float _unboscuredFadeOutDistance;

int _NumCircles;
StructuredBuffer<int> _ActiveCircleIndices;
StructuredBuffer<circleStruct> _CircleBuffer;
StructuredBuffer<ConeEdgeStruct> _ConeBuffer;
//int _fadeType;
float _fadePower;
void CustomCurve_float(float In, out float Out)
{
    Out = In; //fade type 1; linear

#if FADE_SMOOTH
    Out = sin(Out * 1.570796);
#elif FADE_SMOOTHER
    Out = .5 - (cos(Out * 3.1415926) / 2);
#elif FADE_EXP
    Out = pow(Out, _fadePower);
#endif
}


float angleDiff(float ang1, float ang2)
{
    float diff = (ang1 - ang2 + 180) % 360 - 180;
    return diff > _fadeOutDegrees ? diff - 360 : diff;
}
void NoBleedCheck_float(float2 Position, float height, out float Out)
{
    Out = 0;
    for (int i = 0; i < _NumCircles; i++)
    {
        circleStruct circle = _CircleBuffer[_ActiveCircleIndices[i]];
        float distToCircleOrigin = distance(Position, circle.circleOrigin);
        if (distToCircleOrigin < circle.circleRadius)
        {
            float heightDist = abs(height - circle.circleHeight);
            if (heightDist > circle.visionHeight)
                continue;
            float2 relativePosition = Position - circle.circleOrigin;
            float deg = degrees(atan2(relativePosition.y, relativePosition.x));
            
            ConeEdgeStruct previousCone = _ConeBuffer[circle.startIndex];
            float prevAng = previousCone.edgeAngle - .001;
            for (int c = 0; c < circle.numSegments; c++)
            {
                prevAng = previousCone.edgeAngle - .001;
                ConeEdgeStruct currentCone = _ConeBuffer[circle.startIndex + c];

                float degDiff = angleDiff(deg+360, currentCone.edgeAngle);
                float segmentAngle = currentCone.edgeAngle - prevAng;
                
                //if (deg > prevAng && currentCone.edgeAngle > deg)
                if (degDiff > -segmentAngle && degDiff < 0)
                {
                    //float lerpVal = (deg - prevAng) / (currentCone.edgeAngle - prevAng);
                    //float distToConeEnd = lerp(previousCone.length, currentCone.length, lerpVal);
                    float distToConeEnd = currentCone.length;
                    //if (abs(previousCone.length - circle.circleRadius) > .001 || abs(currentCone.length - circle.circleRadius) > .001)
                    if (previousCone.cutShort && currentCone.cutShort)
                    {
                        float2 start = circle.circleOrigin + float2(cos(radians(prevAng)), sin(radians(prevAng))) * previousCone.length;
                        float2 end = circle.circleOrigin + float2(cos(radians(currentCone.edgeAngle)), sin(radians(currentCone.edgeAngle))) * currentCone.length;
                        
                        float a1 = end.y - start.y;
                        float b1 = start.x - end.x;
                        float c1 = a1 * start.x + b1 * start.y;
                    
                        float a2 = Position.y - circle.circleOrigin.y;
                        float b2 = circle.circleOrigin.x - Position.x;
                        float c2 = a2 * circle.circleOrigin.x + b2 * circle.circleOrigin.y;
                    
                        float determinant = (a1 * b2) - (a2 * b1);
                    
                        float x = (b2 * c1 - b1 * c2) / determinant;
                        float y = (a1 * c2 - a2 * c1) / determinant;
                    
                        float2 intercection = float2(x, y);
                        distToConeEnd = distance(intercection, circle.circleOrigin) + _extraRadius;
                    }
                    distToConeEnd = max(distToConeEnd, circle.unobscuredRadius);
                    
                    if (distToCircleOrigin < distToConeEnd)
                    {
                        Out = 1;
                        return;
                    }
                }
                
                previousCone = currentCone;
            }
            if (distToCircleOrigin < circle.unobscuredRadius)
            {
                Out = 1;
                return;
            }
        }
    }
}

void NoBleedSoft_float(float2 Position, float height, out float Out)
{
    Out = 0;
    for (int i = 0; i < _NumCircles; i++)
    {
        circleStruct circle = _CircleBuffer[_ActiveCircleIndices[i]];
        float distToCircleOrigin = distance(Position, circle.circleOrigin);
        float _fadeOutDistance = circle.circleFade;
        if (distToCircleOrigin < circle.circleRadius + _fadeOutDistance)
        {
            float heightDist = abs(height - circle.circleHeight);
            if (heightDist > circle.visionHeight)
            {
                if (heightDist > circle.visionHeight + circle.heightFade)
                    continue;
                heightDist = 1-(heightDist - circle.visionHeight) / circle.heightFade;
            }
            else
                heightDist = 1;

            float2 relativePosition = Position - circle.circleOrigin;
            float deg = degrees(atan2(relativePosition.y, relativePosition.x));
            //deg-=180;
            //Out = (deg)/360;
            //Out = 360%360;
            //return;
            
            //float degAdd = 0;
            ConeEdgeStruct previousCone = _ConeBuffer[circle.startIndex];
            float prevAng = previousCone.edgeAngle - .001;
            for (int c = 0; c < circle.numSegments; c++)
            {
                prevAng = previousCone.edgeAngle - .001;
                ConeEdgeStruct currentCone = _ConeBuffer[circle.startIndex + c];

                float degDiff = angleDiff(deg+360, currentCone.edgeAngle);
                float segmentAngle = currentCone.edgeAngle - prevAng;
                #if OUTER_SOFTEN
                if (degDiff > -segmentAngle && degDiff < 0)
                #elif INNER_SOFTEN
                if (degDiff > -segmentAngle - _fadeOutDegrees && degDiff < _fadeOutDegrees)
                #endif
                {
                    //float lerpVal = clamp(segmentAngle-degDiff, 0, segmentAngle)/segmentAngle;
                    //float distToConeEnd = lerp(previousCone.length, currentCone.length, lerpVal);
                    float distToConeEnd = currentCone.length;
                    float newBlurDistance = (distToConeEnd / circle.circleRadius) * _fadeOutDistance;
                    
                    #if INNER_SOFTEN
                    if (!(degDiff > -segmentAngle && degDiff < 0))
                    {
                        float softDistToConeEnd = distToConeEnd;
                        float softnewBlurDistance = newBlurDistance;

                        float angDiff = degDiff / _fadeOutDegrees;
                        if (degDiff < 0)
                        {
                            angDiff = clamp(((segmentAngle-degDiff) / _fadeOutDegrees), 0, 1);
                        }
                        //float arcLen = (2 * (distToConeEnd * distToConeEnd)) - (2 * distToConeEnd * distToConeEnd * cos(radians(_fadeOutDegrees)));
                        
                        if (previousCone.cutShort)
                        {
                        
                            if (currentCone.cutShort)
                            {
                                softnewBlurDistance = 0;
                                //softDistToConeEnd = 0;
                            }
                            if ((c == 0 || c == circle.numSegments-1))
                            {
                                softnewBlurDistance = distToConeEnd - circle.circleRadius;
                                softDistToConeEnd = circle.circleRadius;
                            }
                            
                            if (distToConeEnd > circle.circleRadius )
                            {
                                softnewBlurDistance = distToConeEnd - circle.circleRadius;
                                softDistToConeEnd = circle.circleRadius;
                            }
                            //if (currentCone.cutShort && !(c == 0 || c == circle.numSegments-1))
                            //{
                                //softnewBlurDistance = 0;
                                //softDistToConeEnd = 0;
                            //}
                        }
                        else
                        {
                            softDistToConeEnd = min(previousCone.length, currentCone.length);
                        }
                        //softnewBlurDistance+= arcLen;

                        if (distToCircleOrigin < softDistToConeEnd + softnewBlurDistance)
                        {
                            if (distToCircleOrigin < softDistToConeEnd)
                            {
                                Out = max(Out, heightDist * cos(angDiff * 1.570796));
                            }
                            Out = max(Out, heightDist * lerp(0, cos(angDiff * 1.570796), clamp(((softDistToConeEnd + _fadeOutDistance) - distToCircleOrigin) / _fadeOutDistance, 0,1)));
                        }
                        previousCone = currentCone;
                        continue;
                    }
                    #endif
                    //Out = 1;
                    //previousCone = currentCone;
                    //continue;
                    //if (abs(previousCone.length - circle.circleRadius) > .001 || abs(currentCone.length - circle.circleRadius) > .001)
                    if (previousCone.cutShort && currentCone.cutShort)
                    {
                        float2 start = circle.circleOrigin + float2(cos(radians(prevAng)), sin(radians(prevAng))) * previousCone.length;
                        float2 end = circle.circleOrigin + float2(cos(radians(currentCone.edgeAngle)), sin(radians(currentCone.edgeAngle))) * currentCone.length;
                        
                        float a1 = end.y - start.y;
                        float b1 = start.x - end.x;
                        float c1 = a1 * start.x + b1 * start.y;
                    
                        float a2 = Position.y - circle.circleOrigin.y;
                        float b2 = circle.circleOrigin.x - Position.x;
                        float c2 = a2 * circle.circleOrigin.x + b2 * circle.circleOrigin.y;
                    
                        float determinant = (a1 * b2) - (a2 * b1);
                    
                        float x = (b2 * c1 - b1 * c2) / determinant;
                        float y = (a1 * c2 - a2 * c1) / determinant;
                    
                        float2 intercection = float2(x, y);
                        distToConeEnd = distance(intercection, circle.circleOrigin);
                        newBlurDistance = 0;
                        if (distToConeEnd > circle.circleRadius)
                        {
                            newBlurDistance = distToConeEnd - circle.circleRadius;
                            distToConeEnd = circle.circleRadius;
                        }
                        distToConeEnd += _extraRadius;
                        newBlurDistance += _extraRadius;
                    }
                    distToConeEnd = max(distToConeEnd, circle.unobscuredRadius);
                    
                    if (distToCircleOrigin < distToConeEnd + newBlurDistance)
                    {
                        if (distToCircleOrigin < distToConeEnd)
                        {
                            Out = 1 * heightDist;
                            return;
                        }
                        Out = max(Out, heightDist * lerp(0, 1, ((distToConeEnd + _fadeOutDistance) - distToCircleOrigin) / _fadeOutDistance));
                        previousCone = currentCone;
                        continue;
                    }
                }
                
                previousCone = currentCone;
            }
            //if (distToCircleOrigin < circle.unobscuredRadius)
            //{
            //    Out = 1;
            //    return;
            //}
            if (distToCircleOrigin < circle.unobscuredRadius + _unboscuredFadeOutDistance)
            {
                if (distToCircleOrigin < circle.unobscuredRadius)
                {
                    Out = 1 * heightDist;
                    return;
                }
                Out = max(Out, heightDist * lerp(1, 0, (distToCircleOrigin - circle.unobscuredRadius)/ _unboscuredFadeOutDistance));
            }
        }
    }
}

void FowHard_float(float2 Position, float height, out float Out)
{
    Out = 0;
    for (int i = 0; i < _NumCircles; i++)
    {
        circleStruct circle = _CircleBuffer[_ActiveCircleIndices[i]];
        float distToCircleOrigin = distance(Position, circle.circleOrigin);
        if (distToCircleOrigin < circle.circleRadius)
        {
            float heightDist = abs(height - circle.circleHeight);
            if (heightDist > circle.visionHeight)
                continue;
            float2 relativePosition = Position - circle.circleOrigin;
            float deg = degrees(atan2(relativePosition.y, relativePosition.x));
            //deg = (deg + 360) % 360;
            
            ConeEdgeStruct previousCone = _ConeBuffer[circle.startIndex];
            float prevAng = previousCone.edgeAngle - .001;
            for (int c = 0; c < circle.numSegments; c++)
            {
                prevAng = previousCone.edgeAngle - .001;
                ConeEdgeStruct currentCone = _ConeBuffer[circle.startIndex + c];

                float degDiff = angleDiff(deg+360, currentCone.edgeAngle);
                float segmentAngle = currentCone.edgeAngle - prevAng;
                
                //if (deg > prevAng && currentCone.edgeAngle > deg)
                if (degDiff > -segmentAngle && degDiff < 0)
                {
                    //float lerpVal = (deg - prevAng) / (currentCone.edgeAngle - prevAng);
                    //float distToConeEnd = lerp(previousCone.length, currentCone.length, lerpVal)
                    float distToConeEnd = currentCone.length;

                    if (previousCone.cutShort && currentCone.cutShort)
                    {
                        float2 start = circle.circleOrigin + float2(cos(radians(prevAng)), sin(radians(prevAng))) * previousCone.length;
                        float2 end = circle.circleOrigin + float2(cos(radians(currentCone.edgeAngle)), sin(radians(currentCone.edgeAngle))) * currentCone.length;
                        
                        float a1 = end.y - start.y;
                        float b1 = start.x - end.x;
                        float c1 = a1 * start.x + b1 * start.y;
                    
                        float a2 = Position.y - circle.circleOrigin.y;
                        float b2 = circle.circleOrigin.x - Position.x;
                        float c2 = a2 * circle.circleOrigin.x + b2 * circle.circleOrigin.y;
                    
                        float determinant = (a1 * b2) - (a2 * b1);
                    
                        float x = (b2 * c1 - b1 * c2) / determinant;
                        float y = (a1 * c2 - a2 * c1) / determinant;
                    
                        float2 intercection = float2(x, y);
                        distToConeEnd = distance(intercection, circle.circleOrigin) + _extraRadius;
                        
                        //to add the cone
                        float2 rotPoint = (start + end) / 2;
                        float2 arcOrigin = rotPoint + (float2(-(end.y - rotPoint.y), (end.x - rotPoint.x)) * 3);
                        float arcLength = distance(start, arcOrigin);
                        float2 newRelativePosition = arcOrigin + normalize(Position - arcOrigin) * arcLength;
                        distToConeEnd += distance(intercection, newRelativePosition) / 2;
                    }
                    distToConeEnd = max(distToConeEnd, circle.unobscuredRadius);
                    
                    if (distToCircleOrigin < distToConeEnd)
                    {
                        Out = 1;
                        return;
                    }
                }
                
                previousCone = currentCone;
            }
            if (distToCircleOrigin < circle.unobscuredRadius)
            {
                Out = 1;
                return;
            }
        }
    }
}

void FowSoft_float(float2 Position, float height, out float Out)
{
    Out = 0;
    for (int i = 0; i < _NumCircles; i++)
    {
        circleStruct circle = _CircleBuffer[_ActiveCircleIndices[i]];
        float distToCircleOrigin = distance(Position, circle.circleOrigin);
        float _fadeOutDistance = circle.circleFade;
        if (distToCircleOrigin < circle.circleRadius + _fadeOutDistance)
        {
            float heightDist = abs(height - circle.circleHeight);
            if (heightDist > circle.visionHeight)
            {
                if (heightDist > circle.visionHeight + circle.heightFade)
                    continue;
                heightDist = 1-(heightDist - circle.visionHeight) / circle.heightFade;
            }
            else
                heightDist = 1;

            float2 relativePosition = Position - circle.circleOrigin;
            float deg = degrees(atan2(relativePosition.y, relativePosition.x));
            //deg = (deg + 360) % 360;
            
            ConeEdgeStruct previousCone = _ConeBuffer[circle.startIndex];
            float prevAng = previousCone.edgeAngle - .001;
            for (int c = 0; c < circle.numSegments; c++)
            {
                prevAng = previousCone.edgeAngle - .001;
                ConeEdgeStruct currentCone = _ConeBuffer[circle.startIndex + c];

                float degDiff = angleDiff(deg+360, currentCone.edgeAngle);
                float segmentAngle = currentCone.edgeAngle - prevAng;
                
                #if OUTER_SOFTEN
                if (degDiff > -segmentAngle && degDiff < 0)
                #elif INNER_SOFTEN
                if (degDiff > -segmentAngle - _fadeOutDegrees && degDiff < _fadeOutDegrees)
                #endif
                {
                    //float lerpVal = clamp((deg - prevAng) / (currentCone.edgeAngle - prevAng),0,1);
                    //float distToConeEnd = lerp(previousCone.length, currentCone.length, lerpVal);
                    float distToConeEnd = currentCone.length;
                    float newBlurDistance = (distToConeEnd / circle.circleRadius) * _fadeOutDistance;
                    
                    #if INNER_SOFTEN
                    if (!(degDiff > -segmentAngle && degDiff < 0))
                    {
                        float softDistToConeEnd = distToConeEnd;
                        float softnewBlurDistance = newBlurDistance;

                        float angDiff = degDiff / _fadeOutDegrees;
                        if (degDiff < 0)
                        {
                            angDiff = clamp(((segmentAngle-degDiff) / _fadeOutDegrees), 0, 1);
                        }
                        //float arcLen = (2 * (distToConeEnd * distToConeEnd)) - (2 * distToConeEnd * distToConeEnd * cos(radians(_fadeOutDegrees)));
                        
                        if (previousCone.cutShort)
                        {
                        
                            if (currentCone.cutShort)
                            {
                                softnewBlurDistance = 0;
                                //softDistToConeEnd = 0;
                            }
                            if ((c == 0 || c == circle.numSegments-1))
                            {
                                softnewBlurDistance = distToConeEnd - circle.circleRadius;
                                softDistToConeEnd = circle.circleRadius;
                            }
                            
                            if (distToConeEnd > circle.circleRadius )
                            {
                                softnewBlurDistance = distToConeEnd - circle.circleRadius;
                                softDistToConeEnd = circle.circleRadius;
                            }
                            //if (currentCone.cutShort && !(c == 0 || c == circle.numSegments-1))
                            //{
                            //    softnewBlurDistance = 0;
                            //    softDistToConeEnd = 0;
                            //}
                        }
                        else
                        {
                            softDistToConeEnd = min(previousCone.length, currentCone.length);
                        }
                        //softnewBlurDistance+= arcLen;

                        if (distToCircleOrigin < softDistToConeEnd + softnewBlurDistance)
                        {
                            if (distToCircleOrigin < softDistToConeEnd)
                            {
                                Out = max(Out, heightDist * cos(angDiff * 1.570796));
                            }
                            Out = max(Out, heightDist * lerp(0, cos(angDiff * 1.570796), clamp(((softDistToConeEnd + _fadeOutDistance) - distToCircleOrigin) / _fadeOutDistance, 0,1)));
                        }
                        previousCone = currentCone;
                        continue;
                    }
                    #endif

                    //if (abs(previousCone.length - circle.circleRadius) > .001 || abs(currentCone.length - circle.circleRadius) > .001)
                    if (previousCone.cutShort && currentCone.cutShort)
                    {
                        //previousCone = currentCone;
                        //continue;
                        float2 start = circle.circleOrigin + float2(cos(radians(prevAng)), sin(radians(prevAng))) * previousCone.length;
                        float2 end = circle.circleOrigin + float2(cos(radians(currentCone.edgeAngle)), sin(radians(currentCone.edgeAngle))) * currentCone.length;
                        
                        float a1 = end.y - start.y;
                        float b1 = start.x - end.x;
                        float c1 = a1 * start.x + b1 * start.y;
                    
                        float a2 = Position.y - circle.circleOrigin.y;
                        float b2 = circle.circleOrigin.x - Position.x;
                        float c2 = a2 * circle.circleOrigin.x + b2 * circle.circleOrigin.y;
                    
                        float determinant = (a1 * b2) - (a2 * b1);
                    
                        float x = (b2 * c1 - b1 * c2) / determinant;
                        float y = (a1 * c2 - a2 * c1) / determinant;
                    
                        float2 intercection = float2(x, y);
                        distToConeEnd = distance(intercection, circle.circleOrigin) + _extraRadius;
                        
                        newBlurDistance = 0;
                        if (distToConeEnd > circle.circleRadius)
                        {
                            newBlurDistance = distToConeEnd - circle.circleRadius;
                            distToConeEnd = circle.circleRadius;
                        }
                        distToConeEnd += _extraRadius;
                        newBlurDistance += _extraRadius;
                        
                        //to add the cone
                        float2 rotPoint = (start + end) / 2;
                        float2 arcOrigin = rotPoint + (float2(-(end.y - rotPoint.y), (end.x - rotPoint.x)) * 3);
                        float arcLength = distance(start, arcOrigin);
                        float2 newRelativePosition = arcOrigin + normalize(Position - arcOrigin) * arcLength;
                        newBlurDistance += distance(intercection, newRelativePosition) / 2;
                    }
                    distToConeEnd = max(distToConeEnd, circle.unobscuredRadius);
                    
                    if (distToCircleOrigin < distToConeEnd + newBlurDistance)
                    {
                        if (distToCircleOrigin < distToConeEnd)
                        {
                            Out = heightDist * 1;
                            return;
                        }
                            Out = max(Out, heightDist * lerp(0, 1, ((distToConeEnd + _fadeOutDistance) - distToCircleOrigin) / _fadeOutDistance));
                            break;
                    }
                }
                
                previousCone = currentCone;
            }
            if (distToCircleOrigin < circle.unobscuredRadius + _unboscuredFadeOutDistance)
            {
                if (distToCircleOrigin < circle.unobscuredRadius)
                {
                    Out = heightDist * 1;
                    return;
                }
                Out = max(Out, heightDist * lerp(1, 0, (distToCircleOrigin - circle.unobscuredRadius)/ _unboscuredFadeOutDistance));
            }
            //if (distToCircleOrigin < circle.unobscuredRadius)
            //{
            //    Out = 1;
            //    return;
            //}
        }
    }
}
#endif
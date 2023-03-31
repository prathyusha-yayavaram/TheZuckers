using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    public LineRenderer myCircle;

    [SerializeField] private int mSteps = 30;
    [SerializeField] private float mRadius = 3.0f;
    [SerializeField] private GameObject EnemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        mRadius = EnemyPrefab.GetComponent<EnemyController>().maxDistance;
        myCircle = GetComponent<LineRenderer>();
    }
        

    // Update is called once per frame
    void Update()
    {
        DrawCircle(mSteps, mRadius);
    }

    void DrawCircle(int steps, float radius)
    {
        myCircle.positionCount = steps;
        
        for (int cStep = 0; cStep < steps; cStep ++)
        {
            float circumProgress = (float)cStep / (steps - 1);
            
            float curRadian = circumProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(curRadian);
            float yScaled = Mathf.Sin(curRadian);
            
            float x = xScaled * radius;
            float y = yScaled * radius;
            Vector3 curPos = new Vector3(x, y, 0) + transform.position;
            myCircle.SetPosition(cStep, curPos);

        }
    }
}

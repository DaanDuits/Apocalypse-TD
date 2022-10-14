using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float camScaleSpeed, camSpeed;
    public float toCamScale = 5;
    public Vector3 toPos = new Vector3(0, 0, -10);

    void Update()
    {
        if (transform.position != toPos && Camera.main.orthographicSize != toCamScale)
        {
            Vector3 posDif = toPos - transform.position;
            transform.position += posDif * camSpeed * Time.deltaTime;

            Vector3 next = transform.position + posDif * camSpeed * Time.deltaTime;

            float dif = toCamScale - Camera.main.orthographicSize;
            Camera.main.orthographicSize += dif * camScaleSpeed * Time.deltaTime;

            float nextF = Camera.main.orthographicSize + dif * camScaleSpeed * Time.deltaTime;

            if (new Vector3(0, 0, -10).magnitude < toPos.magnitude)
            {
                if (LevelBehaviour.MagnitudeAndRoundVector(next) > toPos.magnitude)
                {
                    transform.position = toPos;
                }
            }
            else if (LevelBehaviour.MagnitudeAndRoundVector(next) < toPos.magnitude)
            {
                transform.position = toPos;
            }

            if (5 > toCamScale)
            {
                if (LevelBehaviour.RoundFloat(nextF) < toCamScale)
                {
                    Camera.main.orthographicSize = toCamScale;
                }
            }
            else if (LevelBehaviour.RoundFloat(nextF) > toCamScale)
            {
                Camera.main.orthographicSize = toCamScale;
            }
        }
    }
}

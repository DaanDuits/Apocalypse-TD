using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback, bool mainBase)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback, mainBase);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.mainBase);
        }
    }

    public void FinishedProcessingPath(Vector2[] path, bool succes)
    {
        currentPathRequest.callback(path, succes);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;
        public bool mainBase;

        public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback, bool _mainBase)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            mainBase = _mainBase;
        }
    }
}

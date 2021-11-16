using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BearAI : MonoBehaviour {

    private NavMeshAgent _agent;
    private SphereCollider _sphereCol;
    private Vector3 _target;
    private NavMeshPath _path;
    
    public GameObject navigationArea;
    private Vector3 _navAreaExtents;

    public float visibleRange;
    private bool _canRandomPos = true;
    private float _waitFor = 4f; 

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        var radius = _agent.radius;
        _navAreaExtents = navigationArea.GetComponent<Renderer>().bounds.extents;
        
        gameObject.AddComponent<Rigidbody>();
        _sphereCol = gameObject.AddComponent<SphereCollider>();
        _sphereCol.radius = visibleRange;
        _sphereCol.isTrigger = true;

        _path = NewRandomTarget();
        while (_path == null) {
            _path = NewRandomTarget();
        }
    }

    void Update() {
        var dis = Vector3.Distance(transform.position, _target);
        if (dis < 0.3 && _canRandomPos) {
            _path = NewRandomTarget();
            while (_path == null) {
                _path = NewRandomTarget();
            }
        }
        _agent.SetPath(_path);
        //Debug.Log(_target.x + " " + _target.y + " " + _target.z);
    }

    private NavMeshPath NewRandomTarget() {
        NavMeshPath path = new NavMeshPath();
        _target.x = Random.Range(-_navAreaExtents.x, _navAreaExtents.x);
        _target.y = transform.position.y;
        _target.z = Random.Range(-_navAreaExtents.z, _navAreaExtents.z);
        if (NavMesh.CalculatePath(transform.position, _target, NavMesh.AllAreas, path)){
            return path;
        } else {
            return null;
        }
    }

    private NavMeshPath NewTarget(Vector3 target) {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path)) {
            return path;
        } else {
            return null;
        }
    }

    private void OnTriggerEnter( Collider other ) {
        var go = other.gameObject;
        if (go.CompareTag("tree")) {
            if (go.GetComponent<Tree>()._honey) {
                go.GetComponent<Tree>()._honey = false;
                _canRandomPos = false;

                _target = go.transform.position;
                _path = NewTarget(_target);                     //errore da riguardare

                StartCoroutine("Wait");
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, _target);
        Gizmos.DrawSphere(_target, 0.3f);
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(_waitFor);
        _path = NewRandomTarget();
        _canRandomPos = true;
    }
}

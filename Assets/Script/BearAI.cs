using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BearAI : MonoBehaviour {

    private NavMeshAgent _agent;
    private SphereCollider _sphereCol;
    private Vector3 _target;
    
    public GameObject navigationArea;
    private Vector3 _navAreaExtents;

    public float visibleRange;
    private bool _canRandomPos = true;
    private float _waitFor = 4f; 

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        var radius = _agent.radius;
        _navAreaExtents = navigationArea.GetComponent<Renderer>().bounds.extents;
        _target.x = Random.Range(-_navAreaExtents.x + radius, _navAreaExtents.x - radius);
        _target.y = transform.position.y;
        _target.z = Random.Range(-_navAreaExtents.z + radius, _navAreaExtents.z - radius);

        gameObject.AddComponent<Rigidbody>();
        _sphereCol = gameObject.AddComponent<SphereCollider>();
        _sphereCol.radius = visibleRange;
        _sphereCol.isTrigger = true;
    }

    void Update() {
        var dis = Vector3.Distance(transform.position, _target);
        if (dis < 0.3 && _canRandomPos) {
            NewRandomTarget();
        }
        _agent.destination = _target;
        //Debug.Log(_target.x + " " + _target.y + " " + _target.z);
    }

    private void NewRandomTarget() {
        _target.x = Random.Range(-_navAreaExtents.x, _navAreaExtents.x);
        _target.z = Random.Range(-_navAreaExtents.z, _navAreaExtents.z);
    }

    private void OnTriggerEnter( Collider other ) {
        var go = other.gameObject;
        if (go.CompareTag("tree")) {
            if (go.GetComponent<Tree>()._honey) {
                go.GetComponent<Tree>()._honey = false;
                _target = go.transform.position;
                _canRandomPos = false;

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
        NewRandomTarget();
        _canRandomPos = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BearAI2 : MonoBehaviour {

    private NavMeshAgent _agent = new NavMeshAgent();
    private SphereCollider _sphereCol;
    private Rigidbody _rigidBody;
    private NavMeshPath _path;

    public GameObject navigationArea;
    private Vector3 _navAreaExtents;

    public float visibleRange = 5;
    public float speed = 5;
    public float angularSpeed = 5;
    private float _waitFor = 4f;
    private bool _canRandomPos = true;

    void Start() {
        //Componenti
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _agent.angularSpeed = angularSpeed;
        _sphereCol = gameObject.AddComponent<SphereCollider>();
        _sphereCol.radius = visibleRange;
        _sphereCol.isTrigger = true;
        _rigidBody = gameObject.AddComponent<Rigidbody>();
        _rigidBody.useGravity = false;

        //Variabili
        _navAreaExtents = navigationArea.GetComponent<Renderer>().bounds.extents;        
    }

    void Update() {
        var dis = Vector3.Distance(transform.position, _agent.destination);
        if (dis < 0.3f && _canRandomPos) {
            _path = NewRandomPath();
            while (_path == null) {
                _path = NewRandomPath();
            }
            _agent.SetPath(_path);
        }
    }

    private NavMeshPath NewRandomPath() {
        NavMeshPath path = new NavMeshPath();
        Vector3 target;
        float radius = _agent.radius;
        target.x = Random.Range(-_navAreaExtents.x + radius, _navAreaExtents.x - radius);
        target.y = transform.position.y;
        target.z = Random.Range(-_navAreaExtents.z + radius, _navAreaExtents.z - radius);
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
        return path;
    }

    private void OnTriggerEnter( Collider other ) {
        NavMeshPath path = new NavMeshPath();
        if (other.CompareTag("tree")) {
            if (other.gameObject.GetComponent<Tree>()._honey) {
                _agent.FindClosestEdge(out NavMeshHit hit);
                NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path);
                _agent.SetPath(path);
                _canRandomPos = false;
                StartCoroutine("Wait");
            }
        }
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(_waitFor);
        _canRandomPos = true;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, _agent.destination);
        Gizmos.DrawSphere(_agent.destination, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    public bool Honey, Berry;
    [HideInInspector] public bool _honey, _berry;
    private float _honeySpawnTime, _berrySpawnTime, _waitTime;
    void Start() {
        gameObject.AddComponent<BoxCollider>();
        _honey = Honey;
        _berry = Berry;
        _honeySpawnTime = 10;
        if (Honey) {
            Berry = false;
        }
    }


    void Update() {
        if (!_honey && Honey) {
            _waitTime = _honeySpawnTime;
            StartCoroutine("Wait");
        }
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(_waitTime);
        _honey = true;
    }
}

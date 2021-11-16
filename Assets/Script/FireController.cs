using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {
    
    public void FireWeapon() {
        SendMessageUpwards("StartFire");
    }

    public void EndFireWeapon() {
        SendMessageUpwards("EndFire");
    }
}

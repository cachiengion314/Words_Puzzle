using UnityEngine;
using System.Collections;

public class CRotate : MonoBehaviour {
    public float speed;

	private void Update()
    {
        if(transform.localScale.x < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.02f, transform.localScale.y + 0.02f);
        }
        
    }
}

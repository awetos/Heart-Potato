using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartThrow : MonoBehaviour
	{

    public GameObject throwableHeartPrefab;
    Vector3 direction;
	[SerializeField]Vector3 cursorOffset;
    public float deg;
	[SerializeField]private GameObject cursor;

	void Update() {
		//using update for this to avoid it running multiple time with every click
		if (Input.GetMouseButtonDown(0) && Time.timeScale >0){
            GameObject myHeart = Instantiate(throwableHeartPrefab, transform, false);//spawn heart
            myHeart.transform.SetParent(transform.parent.parent);//move it to root
			myHeart.transform.rotation = Quaternion.Euler(90, 0, -deg);//fix rotation of the heart to match where player is aiming
            myHeart.GetComponent<ThrowableHeart>().SetDirection(new Vector3(0, myHeart.transform.position.y, 0));//set the direction to hearts' forward position to launch it forward
        }
	}

	private void FixedUpdate(){
        Vector3 mousePos = Input.mousePosition;


        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 worldPos = Vector3.zero;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            worldPos = hit.point;
        }

         direction = worldPos - transform.position;


        deg = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		
		cursor.transform.position = worldPos+cursorOffset;
		cursor.transform.rotation = Quaternion.Euler(90, 0, -deg);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, deg, 0);

		

    }
}
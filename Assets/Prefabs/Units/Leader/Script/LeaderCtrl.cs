using UnityEngine;
using System.Collections;

public class THC6_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 0;
	
	// Use this for initialization
	void Start () 
	{						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();
		battle_state = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (Input.GetKey ("1"))  // turn to still state
		{ 		
			anim.SetInteger ("battle", 0);
			battle_state = 0;
		}
		if (Input.GetKey ("2")) // turn to battle state
		{ 
			anim.SetInteger ("battle", 1);
			battle_state = 1;
		}
			
		if (Input.GetKey ("up")) 
		{
			anim.SetInteger ("moving", 1);//walk/run/moving
		}
		else 
			{
				anim.SetInteger ("moving", 0);
			}


		if (Input.GetKey ("down")) //walkback
		{
			anim.SetInteger ("moving", 12);
		}
	
		if (Input.GetMouseButtonDown (0)) { // attack1
			anim.SetInteger ("moving", 2);
		}
		if (Input.GetMouseButtonDown (1)) { // attack2
			anim.SetInteger ("moving", 3);
		}
		if (Input.GetMouseButtonDown (2)) { // attack3
			anim.SetInteger ("moving", 4);
		}
		if (Input.GetKeyDown ("space")) { //jump
			anim.SetInteger ("moving", 6);
		}
		if (Input.GetKeyDown ("c")) { //roar/howl
			anim.SetInteger ("moving", 7);
		}
		if (Input.GetKeyDown ("y")) { //powerhit
			anim.SetInteger ("battle", 1);
			battle_state = 1;
			anim.SetInteger ("moving", 5);
		}


		
		if (Input.GetKeyDown ("u")) //hit
		{ 			  
			battle_state = 1;
			anim.SetInteger ("battle", 1);
				
			int n = Random.Range (0, 2);
			if (n == 1) 
				{
					anim.SetInteger ("moving", 8);
				} 
			else 
				{
					anim.SetInteger ("moving", 9);
				}
		}

		if (Input.GetKeyDown ("k")) { //rising
			anim.SetInteger ("battle", 1);
			battle_state = 1;
			anim.SetInteger ("moving", 15);
		}


		
		if (Input.GetKeyDown ("i")) anim.SetInteger ("moving", 13); //die/fall
		if (Input.GetKeyDown ("o")) anim.SetInteger ("moving", 14); //die2
		}
}




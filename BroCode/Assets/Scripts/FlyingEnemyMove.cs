using UnityEngine;
using System.Collections;

public class FlyingEnemyMove : MonoBehaviour {

    //Patrol Variables
    private Vector3 PointA;

    private Vector3 PointB;

    [SerializeField]
    private Transform Point1;

    [SerializeField]
    private Transform Point2;

    private Vector3 next;

    [SerializeField]
    private float moveSpeed;



    //Player Variables
    private Controller Hero;

    public LayerMask isPlayer;
    public float HeroDistance;
    public bool HeroNear;


	// Use this for initialization
	void Start () {
        //Patrol Points initialized
        PointA = Point1.localPosition;
        PointB = Point2.localPosition;
        next = PointB;

        //Player grabbed
        Hero = FindObjectOfType<Controller>();
	}
	
	// Update is called once per frame
	void Update () {

        //check if Hero is in radius
        HeroNear = Physics2D.OverlapCircle(transform.position, HeroDistance, isPlayer);

        //if hero is near, chase else patrol
        if(HeroNear){
            transform.position = Vector3.MoveTowards(transform.position, Hero.transform.position, moveSpeed * Time.deltaTime);
        }
        else{
            Patrol();
        }
        
	}

    //Move Enemy back and forth between two points
    private void Patrol()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, next, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, next) <= 0.1)
        {
            ChangeDestination();
        }
    }

    //Cycle through patrol points
    private void ChangeDestination()
    {
        next = next != PointA ? PointA : PointB;
    }

}

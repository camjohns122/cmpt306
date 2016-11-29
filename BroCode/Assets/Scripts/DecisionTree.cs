using UnityEngine;
using System.Collections;

public class DecisionTree
{
	public delegate bool Decision();
	public delegate void Action();

	public Decision dec;
	public Action act;

	public DecisionTree left;
	public DecisionTree right;

	public DecisionTree(){
		dec = null;
		act = null;
		left = null;
		right = null;
	}


	//since dec and act are public just set them in ai
	/*public void setDecision( passedDecision) {
		dec = passedDecision;
	}

	public void setAction( passedAction) {
		act = passedAction;
	}*/

	public void setLeft(DecisionTree next){
		left = next;
	}

	public void setRight(DecisionTree next){
		right = next;
	}

	public DecisionTree Search (){
		if (dec != null) {
			if (dec() == true) {
				return left.Search ();
			} else {
				return right.Search ();
			}
		} else if (act != null) {
			 act ();
			return null;
		} else {
			Debug.Log ("Error in tree");
			return null;
		}
	}

}



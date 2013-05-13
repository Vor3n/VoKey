using UnityEngine;
using System.Collections;

public class ObjectiveList : MonoBehaviour {
	
	private UILabel[] TextList;
	public string Header;
	public int HeaderSize;
	public int ObjectiveSize;
	public float StartY = 1f;
	public float VerticalGap = 0.5f;
	public int AmountToDisplay = 5;
	
	
	public string[] Objectives;

	public UIFont font;
	
	
	// Use this for initialization
	void Start () {
		GameObject go = new GameObject("ListHeader");
		
    	UILabel GTHeader =  (UILabel)go.AddComponent(typeof(UILabel));
    	GTHeader.text = Header;
		GTHeader.transform.parent = this.transform;
		GTHeader.font = font;
		GTHeader.depth = 3;
		
		GTHeader.transform.localPosition = new Vector3(100f, -50f, 1f);
		GTHeader.transform.localScale = new Vector3( HeaderSize , HeaderSize , 0);
		
		 string []Objectivess = new string[10];
		Objectivess[0] = "Never";
		Objectivess[1] = "Gonna";
		Objectivess[2] = "Give";
		Objectivess[3] = "You";
		Objectivess[4] = "Up";
		Objectivess[5] = "nEver";
		Objectivess[6] = "GoNNa";
		Objectivess[7] = "Let";
		Objectivess[8] = "U";
		Objectivess[9] = "Down";
		
		Init(Objectivess);
		
	
	}
	
	void OnGUI(){
		
	}
	
	
	/// <summary>
	/// Init the specified ItemsToFind.
	/// </summary>
	/// <param name='ItemsToFind'>
	/// Items to find.
	/// </param>
	public void Init(string[] ItemsToFind)
	{
		
		Objectives = ItemsToFind;
		TextList = new UILabel[Objectives.Length];
		CreateTexts();
		
	}
	
	
	
	
	
	// Update is called once per frame
	void Update () {
		UpdateTexts();
		//Debug.Log("" + RemoveItem("You"));
		
	}
	
	/// <summary>
	/// Creates the number of GUITexts specifies in
	/// AmountToDisplay
	/// </summary>
	public void CreateTexts(){
		
		for (int i =0; i< AmountToDisplay; i++)
		{
			GameObject go = new GameObject(i + "Item");
		
    		UILabel GTHeader =  (UILabel)go.AddComponent(typeof(UILabel));
			GTHeader.transform.parent = this.transform;
			GTHeader.font = font;
			GTHeader.depth = 3;
	    	float yPos = StartY + (-i * VerticalGap);
			GTHeader.transform.localPosition = new Vector3(100f, yPos, 1f);
			
		GTHeader.transform.localScale = new Vector3( ObjectiveSize , ObjectiveSize , 0);
			TextList[i] = GTHeader;
		}
		
	}
	
	
	/// <summary>
	/// Updates the GUITexts.
	/// </summary>
	public void UpdateTexts()
	{
	    if(TextList == null) return;
		for(int i = 0; i < AmountToDisplay; i++){
			string text;
			try{
			 text = Objectives[i];
			}catch{
				text = "";	
			}
				
			UILabel GT = TextList[i];
			GT.text = text;
			
		}
	}
	
	
	public bool RemoveItem(string ItemName)
	{
		bool result = false;
		for(int i = 0; i < AmountToDisplay; i++){
			if((i +1)>Objectives.Length) break;
			if(Objectives[i] == ItemName){
				result = true;	
			}
			
		}
		
		
		if(result){
			string[] TempObjectives = new string[Objectives.Length -1];
			int y = 0;
			for(int i = 0; i< Objectives.Length;i++)
			{
				if(Objectives[i] == ItemName )
				{ 
					
					continue;
				}
				TempObjectives[y] = Objectives[i];
				y++;
			}
			
			Objectives = TempObjectives;
			UpdateTexts();
		}
		return result;
	}
	
	
	
}

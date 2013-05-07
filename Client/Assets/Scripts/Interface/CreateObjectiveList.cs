using UnityEngine;
using System.Collections;

public class CreateObjectiveList : MonoBehaviour {
	
	private GUIText[] TextList;
	public string Header;
	public int HeaderSize;
	public int ObjectiveSize;
	public float StartY = 1f;
	public float VerticalGap = 0.5f;
	public int AmountToDisplay = 5;
	
	
	public string[] Objectives;

	public Font font;
	
	
	// Use this for initialization
	void Start () {
		
		
		
		
		GameObject go = new GameObject("ListHeader");
		
    	GUIText GTHeader =  (GUIText)go.AddComponent(typeof(GUIText));
    	GTHeader.text = Header;
		GTHeader.transform.parent = this.transform;
		GTHeader.font = font;
		
		GTHeader.transform.localPosition = new Vector3(0f, 1.29f, 1f);
		GTHeader.fontSize = HeaderSize;
		
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
		TextList = new GUIText[Objectives.Length];
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
		
    		GUIText GTHeader =  (GUIText)go.AddComponent(typeof(GUIText));
			GTHeader.transform.parent = this.transform;
			GTHeader.font = font;
		
	    	float yPos = StartY + (-i * VerticalGap);
			GTHeader.transform.localPosition = new Vector3(0f, yPos, 1f);
			GTHeader.fontSize = ObjectiveSize;
		
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
				
			GUIText GT = TextList[i];
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

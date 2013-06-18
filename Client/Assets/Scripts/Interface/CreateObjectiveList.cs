using UnityEngine;
using System.Collections;

public class CreateObjectiveList : MonoBehaviour {

    private UILabel[] TextList;
	public string Header;
	public float VerticalGap = 0.5f;
	public int AmountToDisplay = 5;
    public Vector2 textOffset = Vector2.one;
    public UIFont HeaderFont;
    public UIFont ListItemFont;
    private int itemDepthIndex = 10;
	public string[] Objectives;
	
	
	// Use this for initialization
	void Start () {
		GameObject go = new GameObject("ListHeader");
        go.layer = LayerMask.NameToLayer("NGUI");
        UILabel objectivesListHeaderLabel = (UILabel)go.AddComponent(typeof(UILabel));
        objectivesListHeaderLabel.pivot = UIWidget.Pivot.TopLeft;
        objectivesListHeaderLabel.depth = itemDepthIndex - 1;
    	objectivesListHeaderLabel.text = Header;
		objectivesListHeaderLabel.transform.parent = this.transform;
        objectivesListHeaderLabel.font = HeaderFont;
		objectivesListHeaderLabel.transform.localPosition = new Vector3(textOffset.x, 1.29f, 1f);
        objectivesListHeaderLabel.MakePixelPerfect();
        if (Objectives == null) throw new UnityException("No items in the list!");
		else Init(Objectives);
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
		for (int i =0; i< AmountToDisplay && i < TextList.Length - 1; i++)
		{
			GameObject go = new GameObject(i + "Item");
            go.layer = LayerMask.NameToLayer("NGUI");
            UILabel GTHeader = (UILabel)go.AddComponent(typeof(UILabel));

            GTHeader.pivot = UIWidget.Pivot.TopLeft;
			GTHeader.transform.parent = this.transform;
            GTHeader.font = ListItemFont;
            
	    	float yPos = textOffset.y + (-i * VerticalGap);
			GTHeader.transform.localPosition = new Vector3(textOffset.x, yPos, 1f);
            
            GTHeader.MakePixelPerfect();
			TextList[i] = GTHeader;
		}
	}
	
	
	/// <summary>
	/// Updates the GUITexts.
	/// </summary>
	public void UpdateTexts()
	{
	    if(TextList == null) return;
        for (int i = 0; i < AmountToDisplay && i < TextList.Length - 1; i++)
        {
			string text;
			try{
			 text = Objectives[i];
			}catch{
				text = "";	
			}

            UILabel GT = TextList[i];
			GT.text = text;
            GT.MakePixelPerfect();
            GT.depth = itemDepthIndex + i;
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

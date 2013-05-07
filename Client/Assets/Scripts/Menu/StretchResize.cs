using UnityEngine;
using System.Collections;

public class StretchResize : MonoBehaviour {
	
	public bool AnchorTop;
	public bool AnchorLeft;
	public bool AnchorRight;
	public bool AnchorBottom;
	
	public string MarginTop;
	public string MarginLeft;
	public string MarginRight;
	public string MarginBottom;
//	public MarginString Margin;
	public string Height;
	
	public string Width;
	
	float DesiredHeight;
	private Margin Margins;
	
	// Use this for initialization
	void Start () {
		Margins = new Margin();
				
		float _Height = 0f;
		if(Height == ""){
			_Height = this.transform.localScale.y;	
		}	
		else if( Height.Contains("%")){
			Height =  Height.Trim().Replace("%", "");
			
			float.TryParse(Height,out _Height);
			_Height = Screen.height * _Height /100;		
			
		}else {
			 float.TryParse(Height,out _Height);
		
		}
		
		
		float _Width = 0f;
		if(Width == ""){
			_Width = this.transform.localScale.x;	
		}
		else if( Width.Contains("%")){
			Width =  Width.Trim().Replace("%", "");
			float.TryParse(Width,out _Width);
			_Width = Screen.width * _Width /100;		
			
		}
		else {
			float.TryParse(Width, out _Width);
		
		}
		
		if( MarginBottom.Contains("%")){
			string tempMarg = MarginBottom.Trim().Replace("%", "");	
			float.TryParse(tempMarg,out Margins.Bottom);
			Margins.Bottom = Screen.height * Margins.Bottom /100;	
			
		}else if(MarginBottom == ""){
			Margins.Bottom = 0;
			
		}else{
			float.TryParse(MarginBottom, out Margins.Bottom);
			
		}
		
		if( MarginLeft.Contains("%")){
			string tempMarg = MarginLeft.Trim().Replace("%", "");	
			float.TryParse(tempMarg,out Margins.Left);
			Margins.Left = Screen.width * Margins.Left /100;	
			
		}else if(MarginLeft == ""){
			Margins.Left = 0;
			
		}else{
			float.TryParse(MarginLeft, out Margins.Left);
			
		}
		
		if( MarginTop.Contains("%")){
			string tempMarg = MarginTop.Trim().Replace("%", "");	
			float.TryParse(tempMarg,out Margins.Top);
			Margins.Top = Screen.height * Margins.Top /100;	
			
		}else if(MarginTop == ""){
			Margins.Top = 0;
			
		}else{
			float.TryParse(MarginTop, out Margins.Top);
			
		}
		if( MarginRight.Contains("%")){
			string tempMarg = MarginRight.Trim().Replace("%", "");	
			float.TryParse(tempMarg,out Margins.Right);
			Margins.Right = Screen.width * Margins.Right /100;	
			
		}else if(MarginRight == ""){
			Margins.Right = 0;
			
		}else{
			float.TryParse(MarginRight, out Margins.Right);
			
		}
		
		
		
		
		if(AnchorTop && AnchorBottom){
		
			DesiredHeight = (Screen.height - Margins.Top - Margins.Bottom);
			this.transform.localScale = new Vector3 ( this.transform.localScale.x , DesiredHeight,0);
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, -Margins.Top , 0);
			this.transform.localScale = new Vector3 (  this.transform.localScale.x ,this.transform.localScale.y ,0);
				
		}else if( AnchorTop && !AnchorBottom){
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, -Margins.Top , 0);
			this.transform.localScale = new Vector3 ( this.transform.localScale.x , _Height,0);
			
		}else if( AnchorBottom && !AnchorTop){
			this.transform.localScale = new Vector3 ( this.transform.localScale.x , _Height,0);
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, -(Screen.height -_Height - Margins.Bottom) , 0);
			
			
		}else{
			this.transform.localScale = new Vector3( this.transform.localScale.x, _Height , 0);
			
			
		}
		
		if(AnchorLeft && AnchorRight){
			this.transform.localPosition = new Vector3( Margins.Left, this.transform.localPosition.y , 0);
			this.transform.localScale = new Vector3 ( Screen.width - Margins.Left - Margins.Right , this.transform.localScale.y,0);
			
				
		}else if( AnchorLeft){
			this.transform.localPosition = new Vector3(Margins.Left ,this.transform.localPosition.y , 0);
			this.transform.localScale = new Vector3 (  _Width,this.transform.localScale.y ,0);
			
		}else if( AnchorRight){
			this.transform.localScale = new Vector3 (  _Width ,this.transform.localScale.y ,0);
			this.transform.localPosition = new Vector3(Screen.width -_Width - Margins.Right, this.transform.localPosition.y, 0);
			
			
		}else{
			this.transform.localScale = new Vector3( _Width, this.transform.localScale.y , 0);
			
			
		}
	
		
		
		
	}
	
	
}

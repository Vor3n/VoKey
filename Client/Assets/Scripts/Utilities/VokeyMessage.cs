public sealed class VokeyMessage {

    private readonly string name;
    private readonly int value;

    public static readonly VokeyMessage LOGIN_OK = new VokeyMessage (1, "LOGIN_OK");
    public static readonly VokeyMessage LOGIN_FAIL = new VokeyMessage (2, "LOGIN_FAIL");   
	public static readonly VokeyMessage LEVEL_COMPLETED = new VokeyMessage (4, "LEVEL_COMPLETED");
	public static readonly VokeyMessage LEVEL_CANCELED = new VokeyMessage (5, "LEVEL_CANCELED");
	public static readonly VokeyMessage REQUEST_COMPLETE = new VokeyMessage (6, "REQUEST_COMPLETE");

    private VokeyMessage(int value, string name){
        this.name = name;
        this.value = value;
    }

    public override string ToString(){
        return name;
    }
}
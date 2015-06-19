public class GameManager 
{
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }
	public int Points { get; private set; }
	public int Lives { get; set; }
	public int Level { get; set; }
	
	private GameManager()
	{
		Lives = 3;
		Level = 1;
	}
	
	public void Reset()
	{
		Points = 0;
		Lives = 3;
		Level = 1;
	}
	
	public void AddPoints(int points) 
	{
		Points += points;
	}
	
	public void ResetPoints(int points)
	{
		Points = points;
	}
}

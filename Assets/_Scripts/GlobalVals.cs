public class GlobalVals
{
    private static GlobalVals _instance;
    public static GlobalVals Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalVals();
            }
            return _instance;
        }
    }

    public int PlayerCount { get; set; }

    private GlobalVals()
    {
        PlayerCount = 0;
    }

    public void Reset()
    {
        _instance = new GlobalVals();
    }

}
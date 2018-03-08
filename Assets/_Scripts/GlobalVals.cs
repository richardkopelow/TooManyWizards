using System.Collections.Generic;

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
    public List<PlayerPiece.ClassEnum> PlayerClasses { get; set; }

    private GlobalVals()
    {
        PlayerCount = 1;
        PlayerClasses = new List<PlayerPiece.ClassEnum>() {
            PlayerPiece.ClassEnum.None,
            PlayerPiece.ClassEnum.None,
            PlayerPiece.ClassEnum.None,
            PlayerPiece.ClassEnum.None
        };
    }

    public void Reset()
    {
        _instance = new GlobalVals();
    }

}
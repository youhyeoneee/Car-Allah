// 상수 관리 클래스
public static class Constants 
{
    public static readonly int MAX_RANK_LIST = 20;
    public static readonly string RANKING_KEY = "RankDataTest";
}

public static class SceneNames
{
    public static readonly string LobbyScene = "Lobby";
    public static readonly string RacingScene = "Racing";
    public static readonly string RepairShopScene = "RepairShop";
}

public enum CarPartName
{
    ENGINE_OIL,
    TIRE,
    MISSION_OIL,
    WIPER,
    BREAK_OIL,
    TIMING_BELT,
}
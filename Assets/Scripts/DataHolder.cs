public static class DataHolder 
{
    public static int converterTurn { get; set; }
    public static int furmaPosition { get; set; }
    public static int furmaSet { get; set; }
    public static bool furmaInConverter { get; set; }
    public static bool scoopLoad { get; set; }
    public static bool ladleLoad { get; set; }
    public static bool scrapLoaded { get; set; }
    public static bool ironPoured { get; set; }
    public static bool smelting { get; set; }
    public static bool carrierGo { get; set; }
    public static bool steelCarrierReady { get; set; }
    public static bool slagCarrierReady { get; set; }
    public static bool steelMerging { get; set; }
    public static bool slagMerging { get; set; }
    public static bool release { get; set; }
    public static bool steelPoured { get; set; }
    public static bool slagPoured { get; set; } 
    public static string currentCamera { get; set; }

    public static bool[] Steps = new bool[5];

    public static float[] BunkersFilling = new float[6];

    public static float[] UnloadMaterials = new float[4];

    public static float[] PromBunkerMaterials = new float[4];


}


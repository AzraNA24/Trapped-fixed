using System.Collections.Generic;
using UnityEngine;

public class GameOverMessage : MonoBehaviour
{
    private static Dictionary<string, List<string>> gameOverMessages = new Dictionary<string, List<string>>
    {
        { "Aventurine", new List<string> { "Aventurine: 'Ini bahkan cuma hal kecil buatku'",
                                            "Aventurine: 'Yah NT (Nice Try)'",
                                            "Aventurine: 'Pulanglah kau dulu ngadu sama ibu'" } },
        { "ChaengYul", new List<string> { "ChaengYul: 'Hah? Udah selesai? Gampang banget.",
                                            "ChaengYul: 'Kalo gini mah juga bisa menang sambil merem.'",
                                            "ChaengYul: 'Segitu aja kemampuanmu?'" } },
        { "CheokYul", new List<string> { "CheokYul: 'Itu saja yang kamu punya? Kasihan.'",
                                            "CheokYul: 'Aku bahkan nggak perlu try hard.'",
                                            "CheokYul: 'Kalah lagi? Sudah biasa ya? HAHAHA'" } },
        { "JaekYul", new List<string> { "JaekYul: 'Pulanglah kau dulu ngadu sama ibu'",
                                        "JaekYul: 'Utututu aku mulai merasa kasihan, tapi cuma sedikit'",
                                        "JaekYul: 'Hasilnya udah jelas dari awal'" } },
        { "MrRizzler", new List<string> { "MrRizzler: 'Kekalahanmu ini bikin aku makin keren'",
                                            "MrRizzler: 'Aku menang, dan kamu nggak WKWKW.'",
                                            "MrRizzler: 'Coba lagi, tapi belom tentu hasilnya bakal beda HAHAHA'" } },
        { "Polly", new List<string> { "Polly: 'Terbang tinggi, jatuh lebih keras. Seperti kamu barusan.'",
                                        "Polly: 'EZ win'",
                                        "Polly: 'Mungkin kamu butuh lebih dari sekedar keberuntungan.'" } },
        { "Rolly", new List<string> { "Rolly: 'Aku cuma pemanasan, tapi kamu sudah menyerah.'",
                                        "Rolly: 'Seriusan? Ini terlalu mudah.'",
                                        "Rolly: 'Kamu nggak capek kalah terus?'" } },
        { "RollyPolly", new List<string> { "RollyPolly: 'Kacau ini gampang banget.'",
                                            "RollyPolly: 'Hah udah selesai? Gampang banget?!'",
                                            "RollyPolly: 'Merem dikit menang'" } }
    };

    public static string GetRandomGameOverMessage(string enemyName)
    {
        if (gameOverMessages.ContainsKey(enemyName))
        {
            List<string> messages = gameOverMessages[enemyName];
            return messages[Random.Range(0, messages.Count)];
        }
        else
        {
            return "Kamu kalah! Bahkan tuyul bingung kenapa kamu begitu lemah.";
        }
    }
}

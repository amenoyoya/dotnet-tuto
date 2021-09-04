using System;
using CeVIO.Talk.RemoteService2;

namespace CevioTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 【CeVIO AI】起動
            ServiceControl2.StartHost(false);

            // Talkerインスタンス生成
            Talker2 talker = new Talker2();

            // キャスト設定
            talker.Cast = "小春六花";

            // （例）音量設定
            talker.Volume = 100;

            // （例）再生
            SpeakingState2 state = talker.Speak("こんにちは");
            state.Wait();

            // 【CeVIO AI】終了
            ServiceControl2.CloseHost();
        }
    }
}

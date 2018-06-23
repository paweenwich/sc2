using Google.Protobuf.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC2APIProtocol;
using Starcraft2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sc2
{

   


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        public static ISC2Bot bot;// = new TerranBot();
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Application.Run(new RLForm());
        }

        public static IEnumerable<SC2APIProtocol.Action> MasterAgent_MainLoop(GameState gameState)
        {
            SC2GameState gs = new SC2GameState(gameState);
            SC2APIProtocol.Action answer = bot.Update(gs);
            yield return answer;
        }
        public static void RunSC2()
        {
            var userSettings = Sc2SettingsFile.settingsFromUserDir();
            var instanceSettings = Instance.StartSettings.OfUserSettings(userSettings);

            Func<Instance.Sc2Instance> createInstance =
                () => Runner.run(Instance.start(instanceSettings));

            var participants = new Sc2Game.Participant[] {
                    Sc2Game.Participant.CreateParticipant(
                    createInstance(),
                    Race.Terran,
                    //(state => (IEnumerable<SC2APIProtocol.Action>)new SC2APIProtocol.Action[] {})),
                    MasterAgent_MainLoop),
                    Sc2Game.Participant.CreateComputer(Race.Random, Difficulty.Hard)
            };

            var gameSettings =
                Sc2Game.GameSettings.OfUserSettings(userSettings)
                .WithMap(@"Simple64.SC2Map")
                //.WithRealtime(true)
                //.WithStepsize(10)
                ;
            //.WithRealtime(true);

            // Runs the game to the end with the given bots / map and configuration
            try
            {
                var obj = Sc2Game.runGame(gameSettings, participants);
                Runner.run(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

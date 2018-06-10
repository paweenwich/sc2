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

    public class SC2GameState
    {
        public ResponseObservation NewObservation;
        public ResponseGameInfo GameInfo;
        public ResponseObservation LastObservation;
        public List<SC2APIProtocol.Action> LastActions;
        public SC2APIProtocol.Action CurrentAction;
        private SC2GameState()
        {

        }
        public SC2GameState(GameState gameState)
        {
            NewObservation = gameState.NewObservation;
            GameInfo = gameState.GameInfo;
            LastActions = gameState.LastActions.ToList();
            if (gameState.LastObservation != null)
            {
                LastObservation = gameState.LastObservation.Value;
            }
            CurrentAction = new SC2APIProtocol.Action();
        }
        public SC2GameState(String fileName)
        {
            Stream s = new FileStream(fileName, FileMode.Open);
            LoadFrom(s);
            s.Flush();
            s.Close();
        }

        public void LoadFrom(Stream s)
        {
            BinaryReader bw = new BinaryReader(s);
            NewObservation = new ResponseObservation();
            NewObservation.Load(bw);
            GameInfo = new ResponseGameInfo();
            GameInfo.Load(bw);
            LastObservation = new ResponseObservation();
            LastObservation.Load(bw);
            int num = bw.ReadInt32();
            LastActions = new List<SC2APIProtocol.Action>();
            for(int i=0;i<num;i++)
            {
                SC2APIProtocol.Action a = new SC2APIProtocol.Action();
                a.Load(bw);
                LastActions.Add(a);
            }
            CurrentAction = new SC2APIProtocol.Action();
            CurrentAction.Load(bw);
        }
        public void WriteTo(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            NewObservation.Save(bw);
            GameInfo.Save(bw);
            LastObservation.Save(bw);
            bw.Write(LastActions.Count);
            foreach(SC2APIProtocol.Action a in LastActions)
            {
                a.Save(bw);
            }
            CurrentAction.Save(bw);
        }
        /*public override String ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }*/
    }


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
            Application.Run(new Form1());
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

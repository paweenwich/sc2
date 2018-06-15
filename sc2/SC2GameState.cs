using Newtonsoft.Json;
using SC2APIProtocol;
using Starcraft2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public enum SC2Action: int
    {
        NONE=0,RETREAT_TO_FRIEND,
    }

    public interface ISC2SaveLoad
    {
        void Save(BinaryWriter b);
        void Load(BinaryReader b);
    }
    public class SC2UnitAction: ISC2SaveLoad
    {
        public ulong Tag = 0;
        public SC2Action cmd = SC2Action.NONE;
        public SC2APIProtocol.Action action = new SC2APIProtocol.Action();
        public void Save(BinaryWriter b)
        {
            b.Write(Tag);
            b.Write((int)cmd);
            action.Save(b);
        }
        public void Load(BinaryReader b)
        {
            Tag = (ulong)b.ReadUInt64();
            cmd = (SC2Action)b.ReadInt32();
            action.Load(b);
        }
        public override String ToString()
        {
            return String.Format("{0} {1} {2}", Tag, Enum.GetName(typeof(SC2Action), cmd), action.ToStringEx());
        }

    }

    public class SC2GameStateDelta
    {
        public uint GameLoop;
        public float TotalDamageDealt_Life;
        public float TotalDamageTaken_Life;
    }

    public class SC2GameState
    {
        public ResponseObservation NewObservation;
        public ResponseGameInfo GameInfo;
        public ResponseObservation LastObservation;
        public List<SC2APIProtocol.Action> LastActions;
        public SC2APIProtocol.Action CurrentAction;
        public List<SC2UnitAction> AIActions = new List<SC2UnitAction>();
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
            for (int i = 0; i < num; i++)
            {
                SC2APIProtocol.Action a = new SC2APIProtocol.Action();
                a.Load(bw);
                LastActions.Add(a);
            }
            CurrentAction = new SC2APIProtocol.Action();
            CurrentAction.Load(bw);
            AIActions.Load(bw);
        }
        public void WriteTo(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            NewObservation.Save(bw);
            GameInfo.Save(bw);
            LastObservation.Save(bw);
            bw.Write(LastActions.Count);
            foreach (SC2APIProtocol.Action a in LastActions)
            {
                a.Save(bw);
            }
            CurrentAction.Save(bw);
            AIActions.Save(bw);
        }

        public SC2GameStateDelta GetDelta()
        {
            Score lastScore = LastObservation.Observation.Score;
            Score currentScore = NewObservation.Observation.Score;
            SC2GameStateDelta ret = new SC2GameStateDelta()
            {
                GameLoop = NewObservation.Observation.GameLoop - LastObservation.Observation.GameLoop,
                TotalDamageDealt_Life = currentScore.ScoreDetails.TotalDamageDealt.Life - lastScore.ScoreDetails.TotalDamageDealt.Life,
                TotalDamageTaken_Life = currentScore.ScoreDetails.TotalDamageTaken.Life - lastScore.ScoreDetails.TotalDamageTaken.Life,
            };
            return ret;
        }
    }
}

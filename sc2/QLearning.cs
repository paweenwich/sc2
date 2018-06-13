using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public class QState
    {
        public virtual void Save(BinaryWriter b)
        {
            //throw new NotImplementedException("Save");
        }
        public virtual void Load(BinaryReader b)
        {
            //throw new NotImplementedException("Load");
        }
    }

    public class QStringState: QState
    {
        public String data { get; set; }

        public static explicit operator QStringState(String d)  // explicit byte to digit conversion operator
        {
            QStringState ret = new QStringState();
            ret.data = d;
            return ret;
        }

        public override void Save(BinaryWriter b)
        {
            b.Write(data);
        }
        public override void Load(BinaryReader b)
        {
            data = b.ReadString();
        }
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return data.Equals(((QStringState)obj).data);
        }
        public override string ToString()
        {
            return data;
        }
    }

    public class QData: Dictionary<QState, double[]>
    {
        public void Save(BinaryWriter b)
        {
            b.Write(this.Count);
            foreach(QState key in this.Keys)
            {
                key.Save(b);
                this[key].Save(b);
            }
        }
        public void Load<T>(BinaryReader b) where T: QState, new()
        {
            this.Clear();
            int num = b.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                QState k = new T();
                k.Load(b);
                double[] d = SC2Extend.LoadDouble(b);
                this[k] = d;
            }
            
        }
    }

    public class QLearningTuple<T> where T : QState, new()
    {
        public QState prevState = new T();
        public QState nextState = new T();
        public int action;
        public double reward;
        public void Save(BinaryWriter b)
        {
            prevState.Save(b);
            //nextState.Save(b);
            b.Write(action);
            b.Write(reward);
        }
        public void Load(BinaryReader b) 
        {
            prevState = new T();
            prevState.Load(b);
            nextState = new T();
            //nextState.Load(b);
            action = b.ReadInt32();
            reward = b.ReadDouble();
        }
    }


    public class QLearningOption
    {
        public double epsilon = 0.1f;
    }
    public class QLearning<T> where T : QState, new()
    {
        public QLearningTuple<T> data = new QLearningTuple<T>();
        public int[] Actions = new int[] { 0 }; // Default is do nothing
        public Random rand = new Random();
        public QLearningOption option = new QLearningOption();
        public QData qData = new QData();
        public virtual int GetActionIndex(QState state)
        {
            int ret = 0;
            data.prevState = state;
            double r = rand.NextDouble();
            if (r < option.epsilon)
            {
                ret = RandomActionIndex();
            }
            else
            {
                // find max action for this state
                ret = GetBestActionIndex(state);
            }
            data.action = ret;
            return ret;
        }
        public virtual int GetBestAction(QState state)
        {
            return GetActionFromIndex(GetActionIndex(state));
        }
        public virtual int GetActionFromIndex(int index)
        {
            return Actions[index];
        }
        public virtual int RandomActionIndex()
        {
            return Actions[rand.Next(Actions.Length)];
        }
        public virtual int GetBestActionIndex(QState state)
        {
            throw new NotImplementedException("GetBestActionIndex");
        }
        public void Save(BinaryWriter b)
        {
            data.Save(b);
            //b.Write(JsonConvert.SerializeObject(data));
            b.Write(JsonConvert.SerializeObject(Actions));
            b.Write(JsonConvert.SerializeObject(rand));
            b.Write(JsonConvert.SerializeObject(option));
            qData.Save(b);
        }
        public void Load(BinaryReader b)
        {
            data = new QLearningTuple<T>();
            data.Load(b);
            //data = JsonConvert.DeserializeObject<QLearningTuple>(b.ReadString());
            Actions = JsonConvert.DeserializeObject<int[]>(b.ReadString());
            rand = JsonConvert.DeserializeObject<Random>(b.ReadString());
            option = JsonConvert.DeserializeObject<QLearningOption>(b.ReadString());
            qData = new QData();
            qData.Load<T>(b);
        }
    }

    public enum TarranQLearningAction : int
    {
        NONE = 0, RETREAT_TO_FRIEND,
    }

    public class TarranQLearning: QLearning<QStringState>
    {
        public class TarranState : QStringState
        {
        }


        public TarranQLearning()
        {
            Actions = new int[]
            {
                (int)TarranQLearningAction.NONE,(int)TarranQLearningAction.RETREAT_TO_FRIEND,
            };
        }

        
        public override int GetBestActionIndex(QState state)
        {
            //Make sure we have action for this state, if not, create new with random value 
            if (!qData.ContainsKey(state))
            {
                //create random state
                double[] d = new double[Actions.Length];
                d.RandomFill();
                qData[state] =  d;

            }
            // found return best reqard index
            double[] aData = qData[state];
            return Array.IndexOf(aData, aData.Max());
        }
    }

}

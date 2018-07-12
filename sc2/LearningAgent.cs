using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public class LearningAgent  //SARSA
    {
        public Random rand;
        public int numActions;
        public float learningRate = 0.1f;
        public float epsilon = 0.9f;
        public float discountFactor = 0.9f;
        public Dictionary<float[], float[]> qTable = new Dictionary<float[], float[]>(new StateComparer());
        public float[] prevState;
        public int prevAction;
        public LearningAgent(int numActions)
        {
            this.numActions = numActions;
            rand = new Random(this.GetHashCode());
            Reset();
        }
        public void Reset()
        {
            prevState = null;
            prevAction = 0;
        }
        public Bound GetBound(Dictionary<float[], float[]> tab)
        {
            Bound ret = new Bound();
            foreach (float[] data in tab.Values)
            {
                ret.max = Math.Max(ret.max, data.Max());
                ret.min = Math.Min(ret.min, data.Min());
            }
            return ret;
        }
        public int GetBestAction(float[] state) // greedy
        {
            float[] values = GetValues(state);
            float maxValue = values.Max();
            int action = values.ToList().IndexOf(maxValue);
            return action;
        }
        public int GetNextAction(float[] state) //Epsilon greedy
        {
            int action;
            EnsureQTable(state);
            if (rand.NextDouble() > epsilon)
            {
                action = rand.Next(numActions);
            }
            else
            {
                action = GetBestAction(state);
            }
            return action;
        }
        public void EnsureQTable(float[] state)
        {
            if (!qTable.ContainsKey(state))
            {
                // create random one
                List<float> values = new List<float>();
                for (int i = 0; i < numActions; i++)
                {
                    values.Add((float)rand.NextDouble());
                }
                //Console.WriteLine("New State " + state.ToString());
                qTable.Add(state, values.ToArray());
            }
        }
        public float[] GetValues(float[] state)
        {
            EnsureQTable(state);
            return qTable[state];
        }

        public float GetValue(float[] state, int action)
        {
            float[] values = GetValues(state);
            return values[action];
        }
        public int Process(float[] state)
        {
            prevState = state;
            int action = GetNextAction(state);
            prevAction = action;
            Update(prevState, prevAction);
            return action;
        }
        public virtual void Update(float[] state, int action)
        {
            //UpdateETable(prevState, prevAction);
        }
        public virtual void Learn(float[] state, float currentReward)
        {
        }
        public override String ToString()
        {
            return String.Format("QTable {0}", qTable.Count());
        }
    }

    public class QLearningAgent : LearningAgent
    {
        public QLearningAgent(int numActions) : base(numActions)
        {
        }

        public override void Learn(float[] state, float currentReward)
        {
            int action = GetBestAction(state);
            EnsureQTable(state);
            qTable[prevState][prevAction] = ((1 - learningRate) * qTable[prevState][prevAction]) + (learningRate * (currentReward + discountFactor * qTable[state][action]));
            //qTable[state][action] = ((1 - learningRate) * qTable[prevState][prevAction]) + (learningRate * (currentReward + discountFactor * qTable[state][action]));
        }
    }

    public class SARSALearningAgent : LearningAgent
    {
        public Dictionary<float[], float[]> eTable = new Dictionary<float[], float[]>(new StateComparer());

        public SARSALearningAgent(int numActions) : base(numActions)
        {
        }
        public override void Update(float[] state, int action)
        {
            UpdateETable(prevState, prevAction);
        }

        public void UpdateETable(float[] state, int action)
        {
            EnsureETable(state);
            eTable[state][action] += 1;
        }
        public void EnsureETable(float[] state)
        {
            if (!eTable.ContainsKey(state))
            {
                List<float> values = new List<float>();
                for (int i = 0; i < numActions; i++)
                {
                    values.Add(0);
                }
                eTable.Add(state, values.ToArray());
            }
        }
        public override void Learn(float[] state, float currentReward)
        {
            int action = GetNextAction(state);
            float q = (float)(currentReward + discountFactor * GetValue(state, action) - GetValue(prevState, prevAction));
            foreach (float[] s in qTable.Keys)
            {
                EnsureETable(s);
                float[] data = qTable[s];
                float[] edata = eTable[s];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = data[i] + edata[i] * learningRate * q;
                    edata[i] = edata[i] * discountFactor;
                }
            }
        }
        public override String ToString()
        {
            return String.Format("QTable {0} ETable {1}", qTable.Count(), eTable.Count());
        }
    }


}

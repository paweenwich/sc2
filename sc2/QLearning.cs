using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public interface IQState
    {
    }

    public class QData: Dictionary<IQState, int[]>
    {

    }

    public class QLearningTuple 
    {
        public IQState prevState;
        public int action;
        public IQState nextState;
        public double reward;
    }


    public class QLearningOption
    {
        public double epsilon = 0.1f;
    }
    public class QLearning
    {
        public QLearningTuple data = new QLearningTuple();
        public int[] Actions = new int[] { 0 }; // Default is do nothing
        public Random rand = new Random();
        public QLearningOption option = new QLearningOption();
        public QData qData = new QData();
        public virtual int GetActionIndex(IQState state)
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
        public virtual int RandomActionIndex()
        {
            return Actions[rand.Next(Actions.Length)];
        }
        public virtual int GetBestActionIndex(IQState state)
        {
            throw new NotImplementedException("GetBestActionIndex");
        }
    }

    public enum TarranQLearningAction : int
    {
        NONE = 0, RETREAT_TO_FRIEND,
    }

    public class TarranQLearning: QLearning
    {
        public class TarranState : IQState
        {
        }


        public TarranQLearning()
        {
            Actions = new int[]
            {
                (int)TarranQLearningAction.NONE,(int)TarranQLearningAction.RETREAT_TO_FRIEND,
            };
        }

        
        public override int GetBestActionIndex(IQState state)
        {
            //Check if we have this state in data
            if (qData.ContainsKey(state))
            {   
                // found return best reqard index
                int[] aData = qData[state];
                return Array.IndexOf(aData,aData.Max());
            }else
            {
                //
            }
            return 0;
        }
    }

}

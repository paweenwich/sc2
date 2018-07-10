using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sc2
{
    public partial class RLForm : Form
    {
        public Font drawFont = new Font("Arial", 8);
        GridWorld world = new GridWorld(10, 10);
        LearningAgent learningAgent;
        public RLForm()
        {
            InitializeComponent();
            picMain.Width = 500+1;
            picMain.Height = 500+1;
        }

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.DrawLine(SC2ExtendImageData.penBlack, 0, 0, 500, 500);
            world.Draw(g, new Rectangle(0, 0, 500, 500));
            if (learningAgent != null)
            {
                if (chkEView.Checked)
                {
                    Bound bound = learningAgent.GetBound(learningAgent.eTable);
                    foreach (float[] state in learningAgent.eTable.Keys)
                    {
                        Rectangle rect = world.GetRect((int)state[0], (int)state[1], new Rectangle(0, 0, 500, 500));
                        float[] eData = learningAgent.eTable[state];
                        float eMax = eData.Max();
                        int eMaxIndex = eData.ToList().IndexOf(eMax);
                        WorldAction act = Me.IndexToAction(eMaxIndex);
                        int c = (int)bound.map(eMax, 0, 255);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(c,c,c)),rect);
                        g.DrawString(String.Format("{0:0.00}", eMax), drawFont, rect.Left, rect.Top);
                        g.DrawString(String.Format("{0}", act.ToString()), drawFont, rect.Left, rect.Top + 12);

                    }
                }
                else
                {
                    Bound bound = learningAgent.GetBound(learningAgent.qTable);
                    foreach (float[] state in learningAgent.qTable.Keys)
                    {
                        Rectangle rect = world.GetRect((int)state[0], (int)state[1], new Rectangle(0, 0, 500, 500));
                        float[] qData = learningAgent.qTable[state];
                        float qMax = qData.Max();
                        int qMaxIndex = qData.ToList().IndexOf(qMax);
                        WorldAction act = Me.IndexToAction(qMaxIndex);
                        int c = (int)bound.map(qMax, 0, 255);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(c, c, c)), rect);
                        g.DrawString(String.Format("{0:0.00}", qMax), drawFont, rect.Left, rect.Top);
                        g.DrawString(String.Format("{0}", act.ToString()), drawFont, rect.Left, rect.Top + 12);

                    }
                }
            }
            //g.DrawGrid(SC2ExtendImageData.penBlack, ,10,10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Me me = new Me() { pos = { X = 1, Y = 1 } };
            ReachTarget target = new ReachTarget() { pos = { X = 8, Y = 8 } };
            learningAgent = me.learningAgent;
            List<int> nums = new List<int>();
            for (int i = 0; i < 200; i++)
            {

                world.Reset();
                me.Reset();
                learningAgent.Reset();
                me.learningAgent = learningAgent;
                me.pos = new Point{ X = 1, Y = 1 };
                //world.AddObject(new Me() { pos = { X = 1, Y = 1 } });
                world.AddObject(me);
                world.AddObject(target);
                int num = 0;
                while (!world.isEnd())
                {
                    world.Process();
                    world.Learn();
                    //picMain.Refresh();
                    num++;
                }
                nums.Add(num);
                //Console.WriteLine(world.objects[0].ToString());
                //Console.WriteLine(num);
                //break;
            }
            picMain.Refresh();
            Console.WriteLine(nums.ToStringEx().Replace(","," ").Replace("{","").Replace("}",""));
        }

        private void chkEView_CheckedChanged(object sender, EventArgs e)
        {
            picMain.Refresh();
        }
    }

    public enum WorldData: int 
    {
        Moveable = 0,
        Block,
    }

    public enum WorldObjectType: int
    {
        Target = 1,
        Me,
    }

    public class WorldObject
    {
        //public static Random rand = new Random(1414);
        public Point pos;
        public WorldObjectType type;
        public float reward = 0;
        public float prevReward = 0;
        public WorldObject()
        {
            //rand = new Random(this.GetHashCode());
        }
        public virtual void Reset()
        {
            reward = 0;
            prevReward = 0;
        }
        public virtual WorldAction Process(GridWorld world)
        {
            return 0;
        }
        public virtual void Learn(GridWorld world)
        {
        }
        public virtual float[] GetValues(float[] state)
        {
            return new float[0];
        }

        public void Reward(float value)
        {
            prevReward = reward;
            reward += value;
        }
        public float CurrentReward()
        {
            return reward - prevReward;
        }

    }

    public enum WorldAction : int
    {
        None = 0, UP, DOWN, LEFT, RIGHT, END,
    }

    public class StateComparer : IEqualityComparer<float[]>
    {
        public bool Equals(float[] x, float[] y)
        {
            for(int i = 0; i < y.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(float[] x)
        {
            return (int)(x.Sum()*100000);
        }


    }

    public class Bound
    {
        public float min = 1000000;
        public float max = -100000;
        public float map(float value, float toMin, float toMax)
        {
            return ((value - min) / (max - min)) * (toMax - toMin) + toMin;
        }

    }
    public class LearningAgent
    {
        public Random rand;
        public int numActions;
        public float learningRate = 0.1f;
        public float epsilon = 0.9f;
        public float discountFactor = 0.9f;
        public Dictionary<float[], float[]> qTable = new Dictionary<float[], float[]>(new StateComparer());
        public Dictionary<float[], float[]> eTable = new Dictionary<float[], float[]>(new StateComparer());
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
            foreach(float[] data in tab.Values)
            {
                ret.max = Math.Max(ret.max, data.Max());
                ret.min = Math.Min(ret.min, data.Min());
            }
            return ret;
        }
        public int GetNextAction(float[] state) //Epsilon greedy
        {
            int action;
            if (rand.NextDouble() > epsilon)
            {
                action = rand.Next(numActions);
            }else
            {
                float[] values = GetValues(state);
                float maxValue = values.Max();
                action = values.ToList().IndexOf(maxValue);
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
        public void UpdateQTable(float[] state, int action, float q)
        {
            EnsureQTable(state);
            //qTable[state][action] = ((1 - learningRate) * qTable[state][action]) + ((learningRate) * q);
            qTable[state][action] = ((1 - learningRate) * qTable[state][action]) + ((learningRate) * q * eTable[state][action]);
            //eTable[state][action] = eTable[state][action] * discountFactor;
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
        public void UpdateETable(float[] state, int action)
        {
            EnsureETable(state);
            eTable[state][action] += 1;
        }
        public int Process(float[] state)
        {
            prevState = state;
            int action = GetNextAction(state);
            prevAction = action;
            UpdateETable(prevState, prevAction);
            return action;
        }
        public void Learn(float[] state,float currentReward)
        {
            int action = GetNextAction(state);
            float q = (float)(currentReward + discountFactor * GetValue(state, action) - GetValue(prevState, prevAction));
            //UpdateQTable(prevState, prevAction, q);
            // Deval qTable
            foreach (float[] s in qTable.Keys)
            {
                EnsureETable(s);
                float[] data = qTable[s];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = data[i] + eTable[s][i] * learningRate * q;
                }
            }
            // Decay eTable
            foreach (float[] data in eTable.Values)
            {
                for(int i = 0; i < data.Length; i++)
                {
                    data[i] = data[i] * discountFactor;
                }
            }
        }
        public override String ToString()
        {
            return String.Format("QTable {0} ETable {1}", qTable.Count(), eTable.Count());
        }
    }


    public class Me : WorldObject
    {
        public LearningAgent learningAgent;
        public static WorldAction[] actions = new WorldAction[] {
            WorldAction.None, WorldAction.UP, WorldAction.DOWN, WorldAction.LEFT, WorldAction.RIGHT, 
        };
        public Me()
        {
            type = WorldObjectType.Me;
            learningAgent = new LearningAgent(actions.Length);
        }
        public WorldAction GetNextAction(float[] state)
        {
            float[] values = GetValues(state);
            float maxValue = values.Max();
            int index = values.ToList().IndexOf(maxValue);
            return IndexToAction(index);
        }
        public static int ActionToIndex(WorldAction act)
        {
            return actions.ToList().IndexOf(act);
        }
        public static WorldAction IndexToAction(int index)
        {
            return actions[index];
        }
        public override WorldAction Process(GridWorld world)
        {
            float[] state = world.GetState();
            int index =  learningAgent.Process(state);
            return IndexToAction(index);

        }
        public override void Learn(GridWorld world)
        {
            float[] currentState = world.GetState();
            float currentReward = CurrentReward();
            learningAgent.Learn(currentState, currentReward);
        }

        public override String ToString()
        {
            return String.Format("{0} {1}",learningAgent.ToString(),reward);
        }
    }

    public class ReachTarget : WorldObject
    {
        public ReachTarget()
        {
            type = WorldObjectType.Target;
        }
        public override WorldAction Process(GridWorld world)
        {
            foreach(WorldObject obj in world.objects)
            {
                if (obj == this) continue;
                if((obj.pos.X == this.pos.X) && (obj.pos.Y == this.pos.Y) )
                {
                    obj.Reward(1000);
                    return WorldAction.END;
                }
                obj.Reward(-1);
            }
            return WorldAction.None;
        }

    }


    public class GridWorld
    {
        public bool worldEnd = false;
        public int nx;
        public int ny;
        public WorldData[,] data;
        public List<WorldObject> objects = new List<WorldObject>();
        public GridWorld(int nx,int ny)
        {
            this.nx = nx;
            this.ny = ny;
            data = new WorldData[nx, ny];
            Reset();
        }
        public void AddObject(WorldObject obj)
        {
            objects.Add(obj);
        }
        public float[] GetState()
        {
            List<float> ret = new List<float>();
            // Simple X and Y of each object
            foreach (WorldObject obj in objects)
            {
                ret.Add(obj.pos.X);
                ret.Add(obj.pos.Y);
            }
            return ret.ToArray();
        }
        public void Process()
        {
            foreach (WorldObject obj in objects)
            {
                WorldAction action = obj.Process(this);
                if(action != WorldAction.None)
                {
                    DoAction(obj, action);
                }
            }
        }
        public void Learn()
        {
            foreach (WorldObject obj in objects)
            {
                obj.Learn(this);
            }
        }
        public bool isEnd()
        {
            return worldEnd;
        }
        public void Reset()
        {
            worldEnd = false;
            objects.Clear();
            //AddObject(new Me(learningAgent) { pos = { X = 1, Y = 1 } });
            //AddObject(new ReachTarget() { pos = { X = 8, Y = 8 } });
        }
        public void DoAction(WorldObject obj, WorldAction action)
        {
            switch (action)
            {
                case WorldAction.None: break;
                case WorldAction.UP:
                    {
                        if(obj.pos.Y > 0)
                        {
                            obj.pos.Y--;
                        }
                        break;
                    }
                case WorldAction.DOWN:
                    {
                        if (obj.pos.Y < ny -1)
                        {
                            obj.pos.Y++;
                        }
                        break;
                    }
                case WorldAction.LEFT:
                    {
                        if (obj.pos.X > 0)
                        {
                            obj.pos.X--;
                        }
                        break;
                    }
                case WorldAction.RIGHT:
                    {
                        if (obj.pos.X < nx - 1)
                        {
                            obj.pos.X++;
                        }
                        break;
                    }
                case WorldAction.END:
                    {
                        worldEnd = true;
                        break;
                    }
            }

        }
        public Rectangle GetRect(int x,int y, Rectangle rect)
        {
            int sx = (rect.Width / nx);
            int sy = (rect.Height / ny);
            return new Rectangle(x * sx, y * sy, sx, sy);
        }
        public void Draw(Graphics g, Rectangle rect)
        {
            g.DrawGrid(SC2ExtendImageData.penBlack,rect, nx, ny);
            int sx = (rect.Width / nx);
            int sy = (rect.Height / ny);
            for (int x = 0; x < nx; x++)
            {
                for(int y = 0; y < ny; y++)
                {
                    Brush brush = SC2ExtendImageData.drawBrushYellow;
                    switch (data[x,y])
                    {
                        case WorldData.Block: brush = SC2ExtendImageData.drawBrushBlack; break;
                        case WorldData.Moveable: brush = SC2ExtendImageData.drawBrushWhite; break;
                    }
                    g.FillRectangle(brush, rect.Left + 1 + x * sx, rect.Top + 1 + y * sy, sx-2, sy-2);
                }
            }
            foreach(WorldObject w in objects)
            {
                Pen pen = SC2ExtendImageData.penYellow;
                switch (w.type)
                {
                    case WorldObjectType.Me: pen = SC2ExtendImageData.penBlue2; break;
                    case WorldObjectType.Target: pen = SC2ExtendImageData.penViolet2; break;
                }

                g.DrawCircle(pen, rect.Left + w.pos.X * sx + sx / 2, rect.Top + w.pos.Y * sy + sy / 2,sx/2-2);
            }
        }
    }
}

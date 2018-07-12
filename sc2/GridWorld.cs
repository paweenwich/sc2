using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{
    public enum WorldData : int
    {
        Moveable = 0,
        Block,
    }

    public enum WorldObjectType : int
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
            for (int i = 0; i < y.Length; i++)
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
            return (int)(x.Sum() * 100000);
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


    public class Me : WorldObject
    {
        public LearningAgent learningAgent;
        public static WorldAction[] actions = new WorldAction[] {
            WorldAction.None, WorldAction.UP, WorldAction.DOWN, WorldAction.LEFT, WorldAction.RIGHT,
        };
        public Me(LearningAgent la)
        {
            type = WorldObjectType.Me;
            learningAgent = la;
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
            int index = learningAgent.Process(state);
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
            return String.Format("{0} {1}", learningAgent.ToString(), reward);
        }
    }

    public class ReachTarget : WorldObject
    {
        public float reachReward;
        public ReachTarget(float reachReward = 1000f)
        {
            type = WorldObjectType.Target;
            this.reachReward = reachReward;
        }
        public override WorldAction Process(GridWorld world)
        {
            foreach (WorldObject obj in world.objects)
            {
                if (obj == this) continue;
                if ((obj.pos.X == this.pos.X) && (obj.pos.Y == this.pos.Y))
                {
                    obj.Reward(reachReward);
                    return WorldAction.END;
                }
                //obj.Reward(-1);
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
        public GridWorld(int nx, int ny)
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
                if (action != WorldAction.None)
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
                        if (obj.pos.Y > 0)
                        {
                            obj.pos.Y--;
                        }
                        break;
                    }
                case WorldAction.DOWN:
                    {
                        if (obj.pos.Y < ny - 1)
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
        public Rectangle GetRect(int x, int y, Rectangle rect)
        {
            int sx = (rect.Width / nx);
            int sy = (rect.Height / ny);
            return new Rectangle(x * sx, y * sy, sx, sy);
        }
        public void DrawObject(Graphics g, Rectangle rect)
        {
            int sx = (rect.Width / nx);
            int sy = (rect.Height / ny);
            foreach (WorldObject w in objects)
            {
                Pen pen = SC2ExtendImageData.penYellow;
                switch (w.type)
                {
                    case WorldObjectType.Me: pen = SC2ExtendImageData.penBlue2; break;
                    case WorldObjectType.Target: pen = SC2ExtendImageData.penViolet2; break;
                }

                g.DrawCircle(pen, rect.Left + w.pos.X * sx + sx / 2, rect.Top + w.pos.Y * sy + sy / 2, sx / 2 - 2);
            }
        }
        public void Draw(Graphics g, Rectangle rect)
        {
            g.DrawGrid(SC2ExtendImageData.penBlack, rect, nx, ny);
            int sx = (rect.Width / nx);
            int sy = (rect.Height / ny);
            for (int x = 0; x < nx; x++)
            {
                for (int y = 0; y < ny; y++)
                {
                    Brush brush = SC2ExtendImageData.drawBrushYellow;
                    switch (data[x, y])
                    {
                        case WorldData.Block: brush = SC2ExtendImageData.drawBrushBlack; break;
                        case WorldData.Moveable: brush = SC2ExtendImageData.drawBrushWhite; break;
                    }
                    g.FillRectangle(brush, rect.Left + 1 + x * sx, rect.Top + 1 + y * sy, sx - 2, sy - 2);
                }
            }

        }
    }

}

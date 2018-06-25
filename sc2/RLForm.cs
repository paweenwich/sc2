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
        GridWorld world = new GridWorld(10, 10);
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
            //g.DrawGrid(SC2ExtendImageData.penBlack, ,10,10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                int num = 0;
                while (!world.isEnd())
                {
                    world.Process();
                    picMain.Refresh();
                    num++;
                }
                world.Reset();
                Console.WriteLine(num);
            }
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
        public Point pos;
        public WorldObjectType type;
        public virtual WorldAction Process(GridWorld world)
        {
            return 0;
        }
    }

    public enum WorldAction : int
    {
        None = 0, UP, DOWN, LEFT, RIGHT, END,
    }


    public class Me : WorldObject
    {
        public Random rand = new Random();
        public WorldAction[] actions = new WorldAction[] {
            WorldAction.None, WorldAction.UP, WorldAction.DOWN, WorldAction.LEFT, WorldAction.RIGHT, 
        };

        public Me()
        {
            type = WorldObjectType.Me;
        }
        public override WorldAction Process(GridWorld world)
        {
            int index = rand.Next(actions.Length);
            WorldAction act = actions[index];
            //Console.WriteLine(act.ToString());
            return act;
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
                    return WorldAction.END;
                }
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
        public bool isEnd()
        {
            return worldEnd;
        }
        public void Reset()
        {
            worldEnd = false;
            objects.Clear();
            AddObject(new Me() { pos = { X = 1, Y = 1 } });
            AddObject(new ReachTarget() { pos = { X = 8, Y = 8 } });
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

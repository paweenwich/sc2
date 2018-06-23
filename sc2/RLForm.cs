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
            world.AddObject(new Me() { pos = { X = 1, Y = 1 }});
            world.AddObject(new WorldObject() { pos = { X = 9, Y = 9 }, type = WorldObjectType.Target });
        }

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.DrawLine(SC2ExtendImageData.penBlack, 0, 0, 500, 500);
            world.Draw(g, new Rectangle(0, 0, 500, 500));
            //g.DrawGrid(SC2ExtendImageData.penBlack, ,10,10);
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
        None = 0, UP, DOWN, LEFT, RIGHT
    }


    public class Me : WorldObject
    {
        public Random rand = new Random();
        public WorldAction[] actions = new WorldAction[] {
            WorldAction.None, WorldAction.UP, WorldAction.DOWN, WorldAction.LEFT, WorldAction.RIGHT
        };

        public Me()
        {
            type = WorldObjectType.Me;
        }
        public override WorldAction Process(GridWorld world)
        {
            int index = rand.Next(actions.Length);
            WorldAction act = actions[index];
            return act;
        }
    }



    public class GridWorld
    {
        public int nx;
        public int ny;
        public WorldData[,] data;
        public List<WorldObject> objects = new List<WorldObject>();
        public GridWorld(int nx,int ny)
        {
            this.nx = nx;
            this.ny = ny;
            data = new WorldData[nx, ny];
        }
        public void AddObject(WorldObject obj)
        {
            objects.Add(obj);
        }
        public void Process()
        {
            foreach(WorldObject obj in objects)
            {
                obj.Process(this);
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

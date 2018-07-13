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
        public Random rand = new Random();
        public Font drawFont = new Font("Arial", 8);
        GridWorld world = new GridWorld2(10, 10);
        LearningAgent learningAgent;
        public RLForm()
        {
            InitializeComponent();
            picMain.Width = 500+1;
            picMain.Height = 500+1;
            SetLearningAgent(new SARSALearningAgent(Me.actions.Length) { learningRate = 0.1f });
        }

        public void SetLearningAgent(LearningAgent learningAgent)
        {
            this.learningAgent = learningAgent;
            this.Text = String.Format("RL [{0}]",learningAgent.GetType().ToString());
        }

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.DrawLine(SC2ExtendImageData.penBlack, 0, 0, 500, 500);
            //world.Draw(g, new Rectangle(0, 0, 500, 500));
            if (learningAgent != null)
            {
                if (chkMenuETable.Checked && (learningAgent is SARSALearningAgent))
                {
                    SARSALearningAgent l = (SARSALearningAgent)learningAgent;
                    Bound bound = learningAgent.GetBound(l.eTable);
                    foreach (float[] state in l.eTable.Keys)
                    {
                        Point p = world.StateToXY(state);
                        Rectangle rect = world.GetRect(p.X, p.Y, new Rectangle(0, 0, 500, 500));
                        float[] eData = l.eTable[state];
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
                        Point p = world.StateToXY(state);
                        Rectangle rect = world.GetRect(p.X, p.Y, new Rectangle(0, 0, 500, 500));
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
            world.Draw(g, new Rectangle(0, 0, 500, 500));
            
            //g.DrawGrid(SC2ExtendImageData.penBlack, ,10,10);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //learningAgent = new QLearningAgent(Me.actions.Length) { learningRate = 0.1f};
            Me me = new Me(learningAgent) { pos = { X = 1, Y = 1 } };
            ReachTarget target1 = new ReachTarget(1000) { pos = { X = 8, Y = 8 } };
            ReachTarget target2 = new ReachTarget(100) { pos = { X = 4, Y = 5 } };
            List<int> nums = new List<int>();
            float reward = 0;
            for (int i = 0; i < 100; i++)
            {
                world.Reset();
                me.Reset();
                learningAgent.Reset();
                me.pos = new Point{ X = rand.Next(world.nx), Y = rand.Next(world.ny) };
                //world.AddObject(new Me() { pos = { X = 1, Y = 1 } 
                //target2.pos = new Point { X = rand.Next(world.nx), Y = rand.Next(world.ny) };
                world.AddObject(me);
                world.AddObject(target1);
                world.AddObject(target2);
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
                reward += me.reward;
            }
            picMain.Refresh();
            Console.WriteLine(String.Format("{0:0.00} {1}",reward,learningAgent.qTable.Count));
            //Console.WriteLine(nums.ToStringEx().Replace(","," ").Replace("{","").Replace("}",""));
        }


        private void newQLearningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLearningAgent(new QLearningAgent(Me.actions.Length) { learningRate = 0.1f });
            picMain.Refresh();
        }

        private void newSARSAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLearningAgent(new SARSALearningAgent(Me.actions.Length) { learningRate = 0.1f });
            picMain.Refresh();
        }

        private void qTableToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Checked)
            {
                // uncheck other item with the same tag
                //ToolStripItem parent = (ToolStripItem)item.OwnerItem;
                ToolStripMenuItem toolStrip = (ToolStripMenuItem)item.OwnerItem;
                foreach(ToolStripMenuItem x in toolStrip.DropDownItems)
                {
                    if (x == item) continue;
                    x.Checked = false;
                    //Console.WriteLine(x.ToString());
                }
            }
            picMain.Refresh();
            //Console.WriteLine(item.ToString());
        }
    }

}

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
            //world.Draw(g, new Rectangle(0, 0, 500, 500));
            if (learningAgent != null)
            {
                if (chkEView.Checked && (learningAgent is SARSALearningAgent))
                {
                    SARSALearningAgent l = (SARSALearningAgent)learningAgent;
                    Bound bound = learningAgent.GetBound(l.eTable);
                    foreach (float[] state in l.eTable.Keys)
                    {
                        Rectangle rect = world.GetRect((int)state[0], (int)state[1], new Rectangle(0, 0, 500, 500));
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
            world.DrawObject(g, new Rectangle(0, 0, 500, 500));
            
            //g.DrawGrid(SC2ExtendImageData.penBlack, ,10,10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            learningAgent = new QLearningAgent(Me.actions.Length);
            Me me = new Me(learningAgent) { pos = { X = 1, Y = 1 } };
            ReachTarget target1 = new ReachTarget(1000) { pos = { X = 8, Y = 8 } };
            ReachTarget target2 = new ReachTarget(10) { pos = { X = 4, Y = 5 } };
            List<int> nums = new List<int>();
            for (int i = 0; i < 1000; i++)
            {

                world.Reset();
                me.Reset();
                learningAgent.Reset();
                me.pos = new Point{ X = 1, Y = 1 };
                //world.AddObject(new Me() { pos = { X = 1, Y = 1 } });
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
            }
            picMain.Refresh();
            Console.WriteLine(nums.ToStringEx().Replace(","," ").Replace("{","").Replace("}",""));
        }

        private void chkEView_CheckedChanged(object sender, EventArgs e)
        {
            picMain.Refresh();
        }
    }

}

using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2
{

    public class TerranBuildPattern
    {
        public byte[][] pattern;
        public Dictionary<String, Point2D> data;
    }

    public static class TerranData
    {
        public static TerranBuildPattern[] rampPattens = new TerranBuildPattern[] {
                new TerranBuildPattern()
                {
                    pattern = new byte[4][]
                    {
                        new byte[4] {142,143,143,143},
                        new byte[4] {142,143,143,143},
                        new byte[4] {142,142,143,143},
                        new byte[4] {142,142,142,142},
                    },
                    data = new Dictionary<String, Point2D>
                    {
                        { "Rally", new Point2D() {X = +2, Y=-1 } },
                    },
                },
                new TerranBuildPattern()
                {
                    pattern = new byte[4][]
                    {
                        new byte[4] {143,142,142,142},
                        new byte[4] {143,143,143,142},
                        new byte[4] {143,143,143,143},
                        new byte[4] {143,143,143,143},
                    },
                    data = new Dictionary<String, Point2D>
                    {
                        { "Rally", new Point2D() {X = +2, Y=-2 } },
                    },
                },
        };
    }
}

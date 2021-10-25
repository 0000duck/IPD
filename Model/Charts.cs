using System.Collections.Generic;
using IPD.Common;
using Newtonsoft.Json;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

namespace IPD.Model
{
    public class Charts : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static Charts instance = null;
        private List<ReplyData> replyList = new List<ReplyData>();

        public static Charts GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new Charts();
                        }
                    }
                }
                return instance;
            }
        }

        public List<ReplyData> ReplyList { get => replyList; set => UpdateProperty(ref replyList, value); }

        private List<ISeries> series = new List<ISeries>
        {
            new LineSeries<double, LiveChartsCore.SkiaSharpView.Drawing.Geometries.RectangleGeometry>
            {
                Values = new List<double>{1,2,3,4,5,6,7},
                Name="ddd",
                Fill = null,
                LineSmoothness = 1
            },
            new LineSeries<double,LiveChartsCore.SkiaSharpView.Drawing.Geometries.RectangleGeometry>
            {
                Values = new List<double> { -2, 2, 1, 3, -1, 4, 3 },
                Name = "bbb",
                Stroke = new SolidColorPaint(SKColors.DarkOliveGreen, 3),
                Fill = null,
                GeometryStroke = null,
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometrySize = 1
            }
        };

        public List<ISeries> Series { get => series; set => UpdateProperty(ref series, value); }

        public void ChangeValues(string data)
        {
            ReplyRoot v = JsonConvert.DeserializeObject<ReplyRoot>(data);
            ReplyList.Add(v.replyData);
        }

        public void ClearValues()
        {
            ReplyList.Clear();
        }

        public void ChangeCharts(List<int> axis, string type)
        {
            List<ISeries> ns = new List<ISeries>();
            axis.ForEach(v =>
            {
                List<double> list = new List<double>();
                switch (type)
                {
                    case "realPosACS":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.realPosACS[v - 1]);
                        });
                        break;

                    case "realPosMCS":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.realPosMCS[v - 1]);
                        });
                        break;

                    case "realPosPCS":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.realPosPCS[v - 1]);
                        });
                        break;

                    case "torque":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.torque[v - 1]);
                        });
                        break;

                    case "axisAcc":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.axisAcc[v - 1]);
                        });
                        break;

                    case "axisVel":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.axisVel[v - 1]);
                        });
                        break;

                    case "electric":
                        ReplyList.ForEach(c =>
                        {
                            list.Add(c.electric[v - 1]);
                        });
                        break;

                    default:
                        break;
                }

                ns.Add(new LineSeries<double>
                {
                    Values = list,
                    Name = v.ToString() + "轴",
                    Fill = null,
                    LineSmoothness = 1
                });
            });
            System.Diagnostics.Debug.WriteLine(ns.Count);
            Series = ns;
        }
    }

    public class ReplyData
    {
        /// <summary>
        /// </summary>
        public List<double> axisAcc { get; set; }

        /// <summary>
        /// </summary>
        public List<double> axisVel { get; set; }

        /// <summary>
        /// </summary>
        public List<double> electric { get; set; }

        /// <summary>
        /// </summary>
        public List<double> realPosACS { get; set; }

        /// <summary>
        /// </summary>
        public List<double> realPosMCS { get; set; }

        /// <summary>
        /// </summary>
        public List<double> realPosPCS { get; set; }

        /// <summary>
        /// </summary>
        public List<double> realPosUCS { get; set; }

        /// <summary>
        /// </summary>
        public List<double> torque { get; set; }
    }

    public class ReplyRoot
    {
        /// <summary>
        /// </summary>
        public int channel { get; set; }

        /// <summary>
        /// </summary>
        public ReplyData replyData { get; set; }

        /// <summary>
        /// </summary>
        public int robot { get; set; }
    }
}
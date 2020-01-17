using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnalytics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        float[,] xycoords = new float[72, 2];
        float[,] xyavg = new float[57, 2];
        float[,] trcoords = new float[72, 2];
        float[,] avgcoords = new float[60, 2];
        int ichk1 = 0;
        int ichk2 = 0;

        // Code To Retrieve The Data File //
        public void basicData(string file_name10)
        {
            string[] strArr = null;
            char[] splitChar = { ',', ' ' };
            int eid = 0;

            System.IO.StreamReader objRdr1;
            objRdr1 = new System.IO.StreamReader(file_name10);

            do
            {
               

                strArr = objRdr1.ReadLine().Split(splitChar);

                xycoords[eid, 0] = float.Parse(strArr[0]);
                xycoords[eid, 1] = float.Parse(strArr[1]);
                trcoords[eid, 0] = xycoords[eid, 0];
                trcoords[eid, 1] = xycoords[eid, 1];

                eid++;
            } 
            
            while (objRdr1.Peek() != -1);
            objRdr1.Close();

        }

        // Initial Graph Function //
           public void XYGraph()
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            chart1.ChartAreas.Add("area11");
            chart1.Series.Add("People");
            chart1.Series.Add("PeoplePoints");

            chart1.ChartAreas["area11"].AxisX.Minimum = 0;
            chart1.ChartAreas["area11"].AxisX.Interval = 10;

            bool b = Convert.ToBoolean(ichk2);
            if (ichk2 < 61)
            {
                chart1.ChartAreas["area11"].AxisX.Maximum = 60;
            }
            else
            {

                chart1.ChartAreas["area11"].AxisX.Maximum = 72;
            }

            chart1.ChartAreas[0].AxisX.Title = "Period [Months]";

            chart1.ChartAreas["area11"].AxisY.Minimum = 0;
            chart1.ChartAreas["area11"].AxisY.Interval = 50;
            chart1.ChartAreas["area11"].AxisY.Maximum = 250;
            chart1.ChartAreas[0].AxisY.Title = "Arrivals";

            chart1.Series["People"].Color = Color.DeepSkyBlue;

            chart1.Series["People"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series["PeoplePoints"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            chart1.Series["People"].Points.Clear();
            chart1.Series["PeoplePoints"].Points.Clear();

            for (int i = 0; i < ichk1; i++)
            {
             this.chart1.Series["People"].Points.AddXY(xycoords[i, 0], trcoords[i, 1]);
             //this.chart1.Series["PeoplePoints"].Points.AddXY(xycoords[i, 0], xycoords[i, 1]);
            }
            for (int i = 0; i < ichk2; i++)
            {
                //this.chart1.Series["People"].Points.AddXY(xycoords[i, 0], xycoords[i, 1]);
                this.chart1.Series["PeoplePoints"].Points.AddXY(xycoords[i, 0], trcoords[i, 1]);
            }
        }

        // Button To Initiate Process //
        private void btnRead_Click(object sender, EventArgs e)
        {
            string filDir = "C:\\Jeremy\\";
            string filNam;

            openFileDialog1.InitialDirectory = filDir;

            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);


                filNam = openFileDialog1.FileName;
                basicData(filNam);
                sr.Close();
            }
            ichk1 = 59;
            ichk2 = 59;
            XYGraph();
        }



        // Moving Average Function //
        public void AvgGraph()
        {
            chart2.ChartAreas.Clear();
            chart2.Series.Clear();

            chart2.ChartAreas.Add("area11");
            chart2.Series.Add("People");
            chart2.Series.Add("People Points");

            chart2.ChartAreas["area11"].AxisX.Minimum = 0;
            chart2.ChartAreas["area11"].AxisX.Interval = 10;
            chart2.ChartAreas["area11"].AxisX.Maximum = 60;
            chart2.ChartAreas[0].AxisX.Title = "Period [Months]";

            chart2.ChartAreas["area11"].AxisY.Minimum = 0;
            chart2.ChartAreas["area11"].AxisY.Interval = 20;
            chart2.ChartAreas["area11"].AxisY.Maximum = 200;
            chart2.ChartAreas[0].AxisY.Title = "Arrivals";

            chart2.Series["People"].Color = Color.DeepSkyBlue;

            chart2.Series["People"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart2.Series["People Points"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            chart2.Series["People"].Points.Clear();
            chart2.Series["People Points"].Points.Clear();

            for (int i = 0; i < 59; i++)
            {
                this.chart2.Series["People"].Points.AddXY(xycoords[i, 0], xycoords[i, 1]);
                this.chart2.Series["People Points"].Points.AddXY(xycoords[i, 0], xycoords[i, 1]);
            }
        }

        public void movingavg()
        {
            for (int i = 0; i < 57; i++)
            {
                //MessageBox.Show("check" + i);
                xyavg[i, 0] = i+1;
                xyavg[i, 1] = (xycoords[i, 1] + xycoords[(i + 1), 1] + xycoords[(i + 3), 1]) / 3f;
            }


            for (int i = 0; i < 57; i++)
            {
                xycoords[i, 1] = xyavg[i, 1] ;
            }
            return;   
        }

        // Button To Draw Moving Average //
        private void btnAvg_Click(object sender, EventArgs e)
        {
            movingavg();
            AvgGraph();
        }



        // Trend Line Function //
        public void XYTrend()
        {
            chart1.Series.Add("Trend Line");

            chart1.Series["Trend Line"].Color = Color.Red;
         
            chart1.Series["Trend Line"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart1.Series["Trend Line"].Points.Clear();

            for (int i = 0; i < 59; i++)
            {
                this.chart1.Series["Trend Line"].Points.AddXY(i+1, 0.8515*(i+1)+102.56);
            }
        }

        // Button To Draw Trend Line //
        private void btnTrend_Click(object sender, EventArgs e)
        {
            XYTrend();
        }



        // Detrending Series Function //
        public void XYDetrend()
        {
            chart3.ChartAreas.Clear();
            chart3.Series.Clear();

            chart3.ChartAreas.Add("area11");
            chart3.Series.Add("Detrend");
            chart3.Series.Add("Detrend Points");

            chart3.ChartAreas["area11"].AxisX.Minimum = 0;
            chart3.ChartAreas["area11"].AxisX.Interval = 10;
            chart3.ChartAreas["area11"].AxisX.Maximum = 60;
            chart3.ChartAreas[0].AxisX.Title = "Period [Months]";

            chart3.ChartAreas["area11"].AxisY.Minimum = -100;
            chart3.ChartAreas["area11"].AxisY.Interval = 20;
            chart3.ChartAreas["area11"].AxisY.Maximum = 100;
            chart3.ChartAreas[0].AxisY.Title = "Arrivals";

            chart3.Series["Detrend"].Color = Color.DeepSkyBlue;

            chart3.Series["Detrend"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart3.Series["Detrend Points"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            chart3.Series["Detrend"].Points.Clear();
            chart3.Series["Detrend Points"].Points.Clear();

            for (int i = 0; i < 59; i++)
            {
                double calc = 0.0f;

                calc = 0.8515f *(i + 1.0) - 0.8515 * (1.0f);
                calc = trcoords[i, 1] - calc;

                this.chart3.Series["Detrend"].Points.AddXY(trcoords[i, 0], calc-103.41);
                this.chart3.Series["Detrend Points"].Points.AddXY(trcoords[i, 0], calc-103.41);
            }
        }

        // Button To Draw Detrending Series //
        private void btnDetrend_Click(object sender, EventArgs e)
        {
            XYDetrend();
        }



        // Average Monthly Values Function //

        public void XYStd()
        {
            chart4.ChartAreas.Clear();
            chart4.Series.Clear();

            chart4.ChartAreas.Add("area11");
            chart4.Series.Add("Average");

            chart4.ChartAreas["area11"].AxisX.Minimum = 0;
            chart4.ChartAreas["area11"].AxisX.Interval = 1;
            chart4.ChartAreas["area11"].AxisX.Maximum = 12;
            chart4.ChartAreas[0].AxisX.Title = "Period [Months]";

            chart4.ChartAreas["area11"].AxisY.Minimum = 0;
            chart4.ChartAreas["area11"].AxisY.Interval = 50;
            chart4.ChartAreas["area11"].AxisY.Maximum = 200;
            chart4.ChartAreas[0].AxisY.Title = "Arrivals";

            chart4.Series["Average"].Color = Color.DeepSkyBlue;

            chart4.Series["Average"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            chart4.Series["Average"].Points.Clear();

            for (int i = 0; i < 12; i++)
            {
                int cal = 0;
                float cal1 = 0.0f;

                for (int j = 0; j < 5; j++)
                {
                    cal = (i + j * 12);
                    cal1 = cal1 + trcoords[cal, 1];
                }
                cal1 = cal1 / 5.0f;
                this.chart4.Series["Average"].Points.AddXY(trcoords[i, 0], cal1);
            }
        }

        // Button To Draw Average Monthly Values -  With Values ± Standard Deviation //
        private void button1_Click(object sender, EventArgs e)
        {
            XYStd();
        }

        private void btnPred_Click(object sender, EventArgs e)
        {
            ichk1 = 59;
            ichk2 = 72;
            XYGraph();
        }
    }
}

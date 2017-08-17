using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortingApp
{
    public partial class Form1 : Form
    {
        int[] array;
        Thread thread;
        DateTime startTime;
        DateTime endTime;
        string algorithm;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Start()
        {
            startTime = DateTime.Now;
            switch (algorithm)
            {
                case "Quick Sort":
                    QuickSort(array);
                    break;
                case "Insertion Sort":
                    InsertionSort(array);
                    break;
                case "Bubble Sort":
                    BubbleSort(array);
                    break;

                default:
                    break;
            }


            try
            {
                endTime = DateTime.Now;
                Invoke(new ThreadStart(UpadateTotalTime));
            }
            catch (Exception)
            {

                Thread.CurrentThread.Abort();
            }

        }

        private void UpadateTotalTime()
        {
            var totalTime = endTime - startTime;

            label3.Text = string.Format("{0} seconds",totalTime.TotalSeconds);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            if (array!=null)
            {
                var b = new SolidBrush(Color.Black);
                List<int> s = new List<int>();


                var r = new Rectangle[array.Length];

                for (int i = 0; i < r.Length; i++)
                {
                    var rr = array[i];
                    r[i] = new Rectangle(3 + (5 * i), 290 - rr, 2, rr);
                }

                e.Graphics.FillRectangles(b, r);
            }
            
        }
        void BubbleSort(int[] x)
        {
            for (int i = 0; i < x.Length-1; i++)
            {
                for (int j = 0; j < x.Length-1-i; j++)
                {
                    if (x[j]>x[j+1])
                    {
                        Swap(x, j, j+1);

                    }
                }
            }
        }
        public void InsertionSort(int[] a)
        {
            int N = a.Length;
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (a[j] < a[j - 1])
                    {
                        Swap(a, j, j - 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        private int Partition(int[] a, int lo, int hi)
        {
            int i = lo, j = hi + 1;
            while (true)
            {
                while (a[++i] < a[lo])
                {
                    if (i == hi) break;
                }
                while (a[lo] < a[--j])
                {
                    if (j == lo) break;
                }
                if (i >= j)
                {
                    break;
                }
                Swap(a, i, j);
            }
            Swap(a, lo, j);
            return j;
        }
        private void QuickSort(int[] a, int lo, int hi)
        {
            if (hi <= lo) return;
            int j = Partition(a, lo, hi);
            QuickSort(a, lo, j - 1);
            QuickSort(a, j + 1, hi);
        }
        private void QuickSort(int[] a)
        {
            QuickSort(a, 0, a.Length - 1);
        }
        private void Swap(int[] x, int i, int j)
        {
            var t = x[j];
            x[j] = x[i];
            x[i] = t;
            Thread.Sleep(100);

            try
            {
                Invoke(new ThreadStart(UpdatePanel));
            }
            catch (Exception)
            {

                Thread.CurrentThread.Abort();
            }
        }

        private void UpdatePanel()
        {
            panel1.Refresh();
        }
        int [] GetNumbers()
        {
             var numbers = richTextBox1.Text.Split(new[] { '\r', '\n', ' ', ',' },
                StringSplitOptions.RemoveEmptyEntries);

            var t = new int[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                t[i] = int.Parse(numbers[i]);
            }
            return t;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            algorithm = comboBox1.SelectedItem.ToString();
            Console.WriteLine(algorithm);
            

            var numbers = GetNumbers();
            if (numbers.Length!=0)
            {
                panel1.Refresh();
                array = numbers;

                if (thread != null)
                {
                    thread.Abort();
                }
                thread = new Thread(new ThreadStart(Start));
                thread.Start();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var nums =  GetNumbers();
            double sum = 0;
            foreach (var item in nums)
            {
                sum += item;
            }
            MessageBox.Show(string.Format("The mean of the distribution is {0}.",sum/nums.Length));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var nums = GetNumbers();
            var mode = new Dictionary<int, int>();
            foreach (var item in nums)
            {
                if (mode.ContainsKey(item))
                {
                    mode[item]++;
                }
                else
                {
                    mode.Add(item, 1);
                }
            }
            int modeNumber = 0,max = -1;
            foreach (var item in mode.Keys)
            {
                if (mode[item]>max)
                {
                    max = mode[item];
                    modeNumber = item;
                }
            }
            MessageBox.Show(string.Format("The mode of the distribution is {0} and it occured {1} times.", modeNumber,max));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double median;
            var nums = GetNumbers();
            
            if (nums.Length%2==0)
            {
                median = (nums[(nums.Length) / 2-1]  + nums[(nums.Length) / 2])/2.0;
            }
            else
            {
                median = nums[nums.Length / 2];
            }
            

            MessageBox.Show(string.Format("The median of the distribution is {0}.",median ));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "";
            comboBox1.SelectedIndex = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRPuzzleSolver
{
	public partial class Form1 : Form
	{
		public class Cell
		{
			public enum State_t
			{
				Definite,
				Unsure,
				Nope
			}

			public State_t State;
			public byte Level;

			public Cell()
			{
				State = State_t.Unsure;
			}
		}

		private Cell[] matrix;
		int[][] hrule = new int[][]
		{
			new int[] {7, 3, 1, 1, 7},
			new int[] {1, 1, 2, 2, 1, 1},
			new int[] {1, 3, 1, 3, 1, 1, 3, 1},
			new int[] {1, 3, 1, 1, 6, 1, 3, 1},
			new int[] {1, 3, 1, 5, 2, 1, 3, 1},

			new int[] {1, 1, 2, 1, 1},
			new int[] {7, 1, 1, 1, 1, 1, 7},
			new int[] {3, 3},
			new int[] {1, 2, 3, 1, 1, 3, 1, 1, 2},
			new int[] {1, 1, 3, 2, 1, 1},

			new int[] {4, 1, 4, 2, 1, 2},
			new int[] {1,1,1,1,1,4,1,3},
			new int[] {2,1,1,1,2,5},
			new int[] {3,2,2,6,3,1},
			new int[] {1,9,1,1,2,1},

			new int[] {2,1,2,2,3,1},
			new int[] {3,1,1,1, 1,5,1},
			new int[] {1,2,2,5},
			new int[] {7,1,2,1,1,1,3},
			new int[] {1,1,2,1,2,2,1},

			new int[] {1,3,1,4,5,1},
			new int[] {1,3,1,3,10,2},
			new int[] {1,3,1,1,6,6},
			new int[] {1,1,2,1,1,2},
			new int[] {7,2,1,2,5},
		};

		int[][] vrule = new int[][]
		{
			new int[] {7,2,1,1,7},
			new int[] {1,1,2,2,1,1},
			new int[] {1,3,1,3,1,3,1,3,1},
			new int[] {1,3,1,1,5,1,3,1},
			new int[] {1,3,1,1,4,1,3,1},

			new int[] {1,1,1,2,1,1},
			new int[] {7,1,1,1,1,1,7},
			new int[] {1,1,3},
			new int[] {2,1,2,1,8,2,1},
			new int[] {2,2,1,2,1,1,1,2},

			new int[] {1,7,3,2,1},
			new int[] {1,2,3,1,1,1,1,1},
			new int[] {4,1,1,2,6},
			new int[] {3,3,1,1,1,3,1},
			new int[] {1,2,5,2,2},

			new int[] {2,2,1,1,1,1,1,2,1},
			new int[] {1,3,3,2,1,8,1},
			new int[] {6,2,1},
			new int[] {7,1,4,1,1,3},
			new int[] {1,1,1,1,4},

			new int[] {1,3,1,3,7,1},
			new int[] {1,3,1,1,1,2,1,1,4},
			new int[] {1,3,1,4,3,3},
			new int[] {1,1,2,2,2,6,1},
			new int[] {7,1,3,2,1,1},
		};

		public Form1()
		{
			matrix = new Cell[25 * 25];

			for (int i = 0; i < 25 * 25; i++)
			{
				matrix[i] = new Cell();
			}

				Set(3, 3);
			Set(4, 3);
			Set(12, 3);
			Set(13, 3);
			Set(21, 3);

			Set(6, 8);
			Set(7, 8);
			Set(10, 8);
			Set(14, 8);
			Set(15, 8);
			Set(18, 8);

			Set(6, 16);
			Set(11, 16);
			Set(16, 16);
			Set(20, 16);

			Set(3, 21);
			Set(4, 21);
			Set(9, 21);
			Set(10, 21);
			Set(15, 21);
			Set(20, 21);
			Set(21, 21);

			/*
			for (int count = 0; count < 5; count++ )
			{
				for (int i = 0; i < hrule.Length; i++)
					DoRow(i, hrule[i]); 

				for (int i = 0; i < vrule.Length; i++)
					DoCol(i, vrule[i]);
			}
			 * */

#if false
			int x = 0;
			int spares = 0;

			// First add up the numbers to calculate how many variations we have
			foreach (int i in rule)
			{
				spares += i;
			}
			spares += (rule.Length - 1);

			spares = 25 - spares;
			int gaps = rule.Length + 1;

			int[] buf = new int[25];

			MessageBox.Show(String.Format("Detected {0} spares and {1} spaces", spares, gaps));

			int[] gaprules = new int[gaps];
			int permcount = 0;

			while (GetNextPerm(gaps, spares, gaprules))
			{
				x = 0;
				int blockIndex = 0;
				x += gaprules[blockIndex++];
				permcount++;

				foreach (int i in rule)
				{
					int val = i;

					// Draw on the rules
					while (val-- > 0)
						buf[x++]++;
					//Potential(x++, 0);

					x++;
					x += gaprules[blockIndex++];
				}
			}

			x = 0;
			foreach (int i in buf)
			{
				if (permcount == i)
				{
					Set(x++, 0);
				}
				else if (i > 0)
				{
					Potential(x++, 0);
				}
			} 
#endif

			InitializeComponent();
			CreateBitmapAtRuntime();
		}

		void DoRow(int y, int[] rule)
		{
			int[] buf = new int[25];
			int permCount;
			int x;

			Cell[] originals = new Cell[25];

			for (int i = 0; i < 25; i++)
			{
				originals[i] = Get(i, y);
			}

			Go(rule, buf, originals, out permCount);

			x = 0;
			foreach (int i in buf)
			{
				if (permCount == i)
				{
					Set(x++, y);
				}
				else if (i == 0)
				{
					Nope(x++, y);
				}
				else
				{
					SetCertainty(x++, y, (i*255)/permCount);
				}
			}
		}

		void DoCol(int x, int[] rule)
		{
			int[] buf = new int[25];
			int permCount;
			int y;

			Cell[] originals = new Cell[25];

			for (int i = 0; i < 25; i++)
			{
				originals[i] = Get(x, i);
			}

			Go(rule, buf, originals, out permCount);

			y = 0;

			if (permCount > 0)
			foreach (int i in buf)
			{
				if (permCount == i)
				{
					Set(x, y++);
				}
				else if (i == 0)
				{
					Nope(x, y++);
				}
				else
				{
					SetCertainty(x, y++, (i * 255) / permCount);
				}
			}
		}

		private int Go(int[] rule, int[] buf, Cell[] originals, out int permCount)
		{
			int spares = 0;
			int gaps;
			int[] gapBuckets;

			// First add up the numbers  of cells in this rule to calculate how many spare cells we have left
			foreach (int i in rule)
			{
				spares += i;
			}

			// Add the manditory gaps- 1 between each block
			spares += (rule.Length - 1);

			// Remove the number of occupied gaps from the total to get the number of spares
			spares = 25 - spares;

			// Calculate the number of gaps from the rule length (+ 1 at the end...)
			// Allocate new bucket array for gaps to fit in
			gaps = rule.Length;
			gapBuckets = new int[gaps];
			permCount = 0;

			do
			{
				int[] array = BuildArray(rule, gapBuckets, 25);

				bool bSuccess = true;

				// Compare the buf with the array to see if there are any misplaced ones already
				for (int i = 0; i < 25; i++)
				{
					if ((array[i] == 0) && (originals[i].State == Cell.State_t.Definite))
					{
						bSuccess = false;
						break;
					}

					if ((array[i] == 1) && (originals[i].State == Cell.State_t.Nope))
					{
						bSuccess = false;
						break;
					}
				}

				if (bSuccess)
				{
					permCount++;

					for (int i = 0; i < 25; i++)
					{
						buf[i] += array[i];
					}
				}
			}
			while (GetNextPerm(gaps, spares, gapBuckets));
			return spares;
		}

		int[] BuildArray(int[] rule, int[] gapBuckets, int len)
		{
			int x, gapBucketIndex;
			int[] buf = new int[len];

			x = 0;
			gapBucketIndex = 0;
			//x += gapBuckets[gapBucketIndex++];

			foreach (int i in rule)
			{
				int val = i;

				x += gapBuckets[gapBucketIndex++];

				// Draw on the rules
				while (val-- > 0)
					buf[x++]++;
				//Potential(x++, 0);

				x++;
			}

			return buf;
		}

		bool GetNextPerm(int numBuckets, int numTokens, int[] buckets)
		{
			// Like counting in trinary... each iteraton add one to the first bucket,
			// when each bucket overflows add one to the next bucket!

			//bool bRunning = true;

			while (true)
			{
				int sum = 0;
				buckets[0]++;

				for (int bInd = 0; bInd < numBuckets; bInd++)
				{
					if (buckets[bInd] > numTokens)
					{
						if (bInd + 1 == numBuckets)
							return false;
						else
						{
							buckets[bInd] = 0;
							buckets[bInd + 1]++;
						}
					}
				}

				foreach (int i in buckets)
				{
					sum += i;
				}

				if (sum <= numTokens)
				{
					return true;
				}
			}
		}

		public void SetCertainty(int x, int y, int val)
		{
			matrix[x + y * 25].State = Cell.State_t.Unsure;
			matrix[x + y * 25].Level = (byte)val;
		}

		public void Set(int x, int y)
		{
			matrix[x + y * 25].State = Cell.State_t.Definite;
		}

		public Cell Get(int x, int y)
		{
			return matrix[x + y * 25];
		}

		public void Nope(int x, int y)
		{
			matrix[x + y * 25].State = Cell.State_t.Nope;
		}

		public void CreateBitmapAtRuntime()
		{
			Bitmap flag = new Bitmap(500, 500);
			Graphics flagGraphics = Graphics.FromImage(flag);

			int scale = 10;

			for (int x = 0; x < 25; x++)
			{
				//flagGraphics.DrawLine(Pens.Black, x * scale, 0, x * scale, 25 * scale);
				for (int y = 0; y < 25; y++)
				{
					
					if (Cell.State_t.Definite == matrix[x + y * 25].State)
						flagGraphics.FillRectangle(new SolidBrush(Color.Black), x * scale, y * scale, scale, scale);
#if false
					else
						flagGraphics.FillRectangle(new SolidBrush(Color.White), x * scale, y * scale, scale, scale);
#endif
#if true
					else if (Cell.State_t.Nope == matrix[x + y * 25].State)
						flagGraphics.FillRectangle(new SolidBrush(Color.Pink), x * scale, y * scale, scale, scale);
					else if (Cell.State_t.Unsure == matrix[x + y * 25].State)
						flagGraphics.FillRectangle(new SolidBrush(Color.FromArgb(255 - matrix[x + y * 25].Level, 255 - matrix[x + y * 25].Level, 255)), x * scale, y * scale, scale, scale);
#endif
					/*
					if (2 == matrix[x + y*25])
					{
						flagGraphics.FillRectangle(Brushes.Black, x*scale, y*scale, scale, scale);
					}

					if (1 == matrix[x + y * 25])
					{
						flagGraphics.FillRectangle(Brushes.Blue, x * scale, y * scale, scale, scale);
					}


					if (3 == matrix[x + y * 25])
					{
						flagGraphics.FillRectangle(Brushes.Red, x * scale, y * scale, scale, scale);
					}
					 */
				}
			}

#if true
			for (int y = 0; y <= 25; y++)
			{
				flagGraphics.DrawLine(Pens.Black, 0, y * scale, 25 * scale, y * scale);
			}

			for (int x = 0; x <= 25; x++)
			{
				flagGraphics.DrawLine(Pens.Black, x * scale, 0, x * scale, 25 * scale);
			}
#endif

			pictureBox1.Image = flag;
		}

		private int interationsCount = 0;

		private void button1_Click(object sender, EventArgs e)
		{
			if (!backgroundWorker1.IsBusy)
			{
				backgroundWorker1.RunWorkerAsync();
				button1.Enabled = false;
			}

			//CreateBitmapAtRuntime();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			int total = hrule.Length + vrule.Length;

			for (int i = 0; i < hrule.Length; i++)
			{
				DoRow(i, hrule[i]);
				CreateBitmapAtRuntime();

				//(sender as BackgroundWorker).ReportProgress(10);

				backgroundWorker1.ReportProgress((i * 100) / total, null);
			}

			for (int i = 0; i < vrule.Length; i++)
			{
				DoCol(i, vrule[i]);
				CreateBitmapAtRuntime();

				backgroundWorker1.ReportProgress(((hrule.Length+i) * 100) / total);
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			progressBar1.Value = 100;

			label1.Text = String.Format("{0} iterations", ++interationsCount);

			button1.Enabled = true;
		}
	}
}
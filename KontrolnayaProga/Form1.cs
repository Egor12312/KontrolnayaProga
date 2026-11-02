using System;
using System.Drawing;
using System.Windows.Forms;

namespace Laboratornaya1 {
  public partial class Form1 : Form {
    public Form1()
    {
      InitializeComponent();
    }

    private void menuItemDiscriminant_Click(object sender, EventArgs e)
    {
      Solve(true);
    }

    private void menuItemVieta_Click(object sender, EventArgs e)
    {
      Solve(false);
    }

    private void buttonPlot_Click(object sender, EventArgs e)
    {
      double a, b, c;
      if (!ReadCoefficients(out a, out b, out c)) return;
      DrawGraph(a, b, c);
    }

    private void menuItemClear_Click(object sender, EventArgs e)
    {
      textBoxA.Text = "";
      textBoxB.Text = "";
      textBoxC.Text = "";
      labelResult.Text = "";
      pictureBoxGraph.Image = null;
      pictureBoxGraph.Refresh();
    }

    private void Solve(bool useDiscriminant)
    {
      double a, b, c;
      if (!ReadCoefficients(out a, out b, out c)) return;

      if (a == 0)
      {
        MessageBox.Show("Коэффициент a не может быть 0", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      double[] roots;
      string methodName;

      if (useDiscriminant)
      {
        roots = SolveByDiscriminant(a, b, c);
        methodName = "Дискриминант";
      }
      else
      {
        roots = SolveByVieta(a, b, c);
        methodName = "Виета";
      }

      labelResult.Text = BuildResultText(roots, methodName);
      DrawGraph(a, b, c);
    }

    private string BuildResultText(double[] roots, string methodName)
    {
      string text;
      text = "Метод: " + methodName + Environment.NewLine;

      if (roots.Length == 0)
      {
        text += "Корней нет";
      }
      else if (roots.Length == 1)
      {
        text += "x = " + roots[0].ToString("F3");
      }
      else
      {
        text += "x1 = " + roots[0].ToString("F3") + Environment.NewLine;
        text += "x2 = " + roots[1].ToString("F3");
      }

      return text;
    }

    private bool ReadCoefficients(out double a, out double b, out double c)
    {
      if (!double.TryParse(textBoxA.Text, out a))
      {
        MessageBox.Show("Неверное значение a", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        b = 0; c = 0;
        return false;
      }

      if (!double.TryParse(textBoxB.Text, out b))
      {
        MessageBox.Show("Неверное значение b", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        c = 0;
        return false;
      }

      if (!double.TryParse(textBoxC.Text, out c))
      {
        MessageBox.Show("Неверное значение c", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
      }

      return true;
    }

    private double[] SolveByDiscriminant(double a, double b, double c)
    {
      double D = b * b - 4 * a * c;

      if (D < 0) return new double[0];
      if (D == 0) return new double[] { -b / (2 * a) };

      double sqrtD = Math.Sqrt(D);
      return new double[]
      {
                (-b + sqrtD) / (2 * a),
                (-b - sqrtD) / (2 * a)
      };
    }

    private double[] SolveByVieta(double a, double b, double c)
    {
      double sum = -b / a;
      double product = c / a;
      double D = sum * sum - 4 * product;

      if (D < 0) return new double[0];
      if (D == 0) return new double[] { sum / 2 };

      double sqrtD = Math.Sqrt(D);
      return new double[]
      {
                (sum + sqrtD) / 2,
                (sum - sqrtD) / 2
      };
    }

    private void DrawGraph(double a, double b, double c)
    {
      int w = pictureBoxGraph.Width;
      int h = pictureBoxGraph.Height;
      if (w <= 0 || h <= 0) return;

      Bitmap bmp = new Bitmap(w, h);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        int cx;
        int cy;
        double xRange;
        double yMax;
        double scaleX;
        double scaleY;
        double y;
        float px;
        float py;

        g.Clear(Color.White);

        cx = w / 2;
        cy = h / 2;

        xRange = 10;
        yMax = Math.Abs(a) * xRange * xRange +
                      Math.Abs(b) * xRange +
                      Math.Abs(c);
        if (yMax < 1) yMax = 1;

        scaleX = cx / xRange;
        scaleY = cy / yMax;

        g.DrawLine(Pens.LightGray, 0, cy, w, cy);
        g.DrawLine(Pens.LightGray, cx, 0, cx, h);

        Pen pen = new Pen(Color.Blue, 2);
        PointF? prev = null;

        for (double x = -xRange; x <= xRange; x += 0.05)
        {
          y = a * x * x + b * x + c;
          px = (float)(cx + x * scaleX);
          py = (float)(cy - y * scaleY);

          if (py < -h || py > 2 * h) { prev = null; continue; }

          PointF cur = new PointF(px, py);
          if (prev != null) g.DrawLine(pen, prev.Value, cur);
          prev = cur;
        }
      }

      pictureBoxGraph.Image = bmp;
    }
  }
}
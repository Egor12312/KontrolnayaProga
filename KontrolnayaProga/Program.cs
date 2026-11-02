using System;
using System.Windows.Forms;

namespace Laboratornaya1 {
  internal static class Program {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.Run(new Form1());
    }
  }
}
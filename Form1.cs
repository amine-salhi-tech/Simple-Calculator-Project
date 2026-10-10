using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Calculator_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (textDisplay.Text == "0")
                textDisplay.Text = "";

            textDisplay.Text += btn.Text;
        }

        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (textDisplay.Text == "0" || textDisplay.Text == "")
            {
                textDisplay.Text = "0.";
            }

            char[] operation = new char[4] { '+', '-', '×', '÷' };

            foreach (char o in operation)
            {
                if (textDisplay.Text[textDisplay.Text.Length - 1] == o || textDisplay.Text[textDisplay.Text.Length - 1] == '.')
                    return;
            }

            textDisplay.Text += ".";
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            char[] operation = new char[4] { '+', '-', '×', '÷' };

            foreach (char o in operation)
            {
                if (textDisplay.Text[textDisplay.Text.Length - 1] == o || textDisplay.Text[textDisplay.Text.Length - 1] == '.') 
                    return;
            }

            textDisplay.Text += btn.Text;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (textDisplay.Text == "")
                return;

            textDisplay.Text = textDisplay.Text.Remove(textDisplay.Text.Length - 1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textDisplay.Clear();
        }

        private List<string> ConvertExpressionToList()
        {
            return Regex.Matches(textDisplay.Text, @"\d+(\.\d+)?|[+\-×÷]").Cast<Match>().Select(M => M.Value).ToList();
        }

        private bool HasMultyDecimalPoints()
        {
            string[] Numbers = Regex.Split(textDisplay.Text, @"[\+\-×\÷]");
            int Counter = 0;

            foreach (string n in Numbers)
            {
                Counter = 0;

                foreach (char c in n) 
                {
                    if (c == '.') Counter++;
                }

                if (Counter >= 2)
                    return true;
            }

            return false;
        }

        private bool IsExpressionValid()
        {
            if (!HasMultyDecimalPoints()) 
                return true;
            else
                return false;
        }

        private bool ProcessMultiplicationAndDivision(List<string> Expression)
        {
            for (int i = 1; i < Expression.Count - 1; i++)
            {
                if (Expression[i] == "÷")
                {
                    if (Expression[i + 1] == "0")
                    {
                        MessageBox.Show("You cannot divide by zero.", "Mathimatical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    Expression[i - 1] = (double.Parse(Expression[i - 1]) / double.Parse(Expression[i + 1])).ToString();
                    Expression.RemoveAt(i);
                    Expression.RemoveAt(i);
                    i--;
                }

                if (Expression[i] == "×")
                {
                    Expression[i - 1] = (double.Parse(Expression[i - 1]) * double.Parse(Expression[i + 1])).ToString();
                    Expression.RemoveAt(i);
                    Expression.RemoveAt(i);
                    i--;
                }
            }

            return true;
        }

        private bool ProcessAdditionAndSubtraction(List<string> Expression)
        {
            for (int i = 1; i < Expression.Count - 1; i++)
            {
                if (Expression[i] == "+")
                {
                    Expression[i - 1] = (double.Parse(Expression[i - 1]) + double.Parse(Expression[i + 1])).ToString();
                    Expression.RemoveAt(i);
                    Expression.RemoveAt(i);
                    i--;
                }

                if (Expression[i] == "-")
                {
                    Expression[i - 1] = (double.Parse(Expression[i - 1]) - double.Parse(Expression[i + 1])).ToString();
                    Expression.RemoveAt(i);
                    Expression.RemoveAt(i);
                    i--;
                }
            }

            return true;
        }

        private void DisplayResult(string Result)
        {
            textDisplay.Text = Result;
        }

        private void CalculateResult(List<string> Expression)
        {
            if (!ProcessMultiplicationAndDivision(Expression))
                return;

            if (!ProcessAdditionAndSubtraction(Expression))
                return;

            DisplayResult(Expression[0]);
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            List<string> Expression = ConvertExpressionToList();

            if (IsExpressionValid())
            {
                CalculateResult(Expression);
            }
            else
            {
                MessageBox.Show("The expression is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}

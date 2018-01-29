using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;

namespace Project1
{
    [Activity(Label = "Project1", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        Stack<double> CalcS = new Stack<double>();
        char Operation;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.myButton);

           // button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            Button button1 = FindViewById<Button>(Resource.Id.button1);
            Button button2 = FindViewById<Button>(Resource.Id.button2);
            Button button3 = FindViewById<Button>(Resource.Id.button3);
            Button button4 = FindViewById<Button>(Resource.Id.button4);
            Button button5 = FindViewById<Button>(Resource.Id.button5);
            Button button6 = FindViewById<Button>(Resource.Id.button6);
            Button button7 = FindViewById<Button>(Resource.Id.button7);
            Button button8 = FindViewById<Button>(Resource.Id.button8);
            Button button9 = FindViewById<Button>(Resource.Id.button9);
            Button button0 = FindViewById<Button>(Resource.Id.button0);
            Button buttonPlus = FindViewById<Button>(Resource.Id.buttonPlus);
            Button buttonMin = FindViewById<Button>(Resource.Id.buttonMin);
            Button buttonDiv = FindViewById<Button>(Resource.Id.buttonDiv);
            Button buttonMult = FindViewById<Button>(Resource.Id.buttonMult);
            Button buttonClear = FindViewById<Button>(Resource.Id.Clear);
            Button buttonDelete = FindViewById<Button>(Resource.Id.Delete);
            Button buttonEqual = FindViewById<Button>(Resource.Id.Equal);

            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            button0.Click += Button_Click;
            buttonPlus.Click += OP_Button_Click;
            buttonMin.Click += OP_Button_Click;
            buttonDiv.Click += OP_Button_Click;
            buttonMult.Click += OP_Button_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonDelete.Click += Del_Button_Click;
            buttonEqual.Click += ButtonEqual_Click;


        }

        private void Del_Button_Click(object sender, EventArgs e)
        {
            Button ViewButton = sender as Button;
            TextView output = FindViewById<TextView>(Resource.Id.textView1);
            string temp;
            temp = output.Text;
            if (temp.Length > 0)
            {
                temp = temp.Substring(0, temp.Length - 1);
                output.Text = temp;
            }
        }

        private void OP_Button_Click(object sender, EventArgs e)
        {
            Button ViewButton = sender as Button;
            if (CalcS.Count == 0)
            { 
                if (ViewButton != null)
                {
                    TextView output = FindViewById<TextView>(Resource.Id.textView1);
                    CalcS.Push(System.Convert.ToDouble(output.Text));
                    output.Text += ViewButton.Text;
                    if (output.Text == "+")
                    {
                        Operation = '+';
                    }
                    else if (output.Text == "-")
                    {
                        Operation = '-';
                    }
                    else if (output.Text == "/")
                    {
                        Operation = '/';
                    }
                    else
                    {
                        Operation = '*';
                    }
                    output.Text = "";
                }
             }
        }

        private void ButtonEqual_Click(object sender, System.EventArgs e)
        {
            //Declare variables to handle the input and result
            //Operation handles holding the inputted operator
            double Input1 = 0.0;
            double Input2 = 0.0;
            double Result = 0.0;
            TextView output = FindViewById<TextView>(Resource.Id.textView1);
            
            //Handles our operations
            if(double.TryParse(output.Text, out Result) == false)
            {
                CalcS.Push(System.Convert.ToDouble(output.Text));
                output.Text = "";
                //Once enter is pressed, preform operation
                Input1 = CalcS.Pop();
                Input2 = CalcS.Pop();
                if (Operation == '+')
                {
                    output.Text = (Input1 + Input2).ToString();
                }
                else if (Operation == '-')
                {
                    output.Text = (Input1 - Input2).ToString();
                }
                else if (Operation == '/')
                {
                    output.Text = (Input1 / Input2).ToString();
                }
                else
                {
                    output.Text = (Input1 * Input2).ToString();
                }
            }
        }

        private void ButtonClear_Click(object sender, System.EventArgs e)
        {
            TextView output = FindViewById<TextView>(Resource.Id.textView1);
            output.Text = "";
        }


        private void Button_Click(object sender, System.EventArgs e)
        {
            Button ViewButton = sender as Button;
            if(ViewButton != null)
            {
                TextView output = FindViewById<TextView>(Resource.Id.textView1);
                output.Text += ViewButton.Text;
            }
        }

        
    }
}


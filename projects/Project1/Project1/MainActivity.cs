using Android.App;
using Android.Widget;
using Android.OS;

namespace Project1
{
    [Activity(Label = "Project1", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

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
            Button buttonSign = FindViewById<Button>(Resource.Id.SignChange);
            Button buttonDot = FindViewById<Button>(Resource.Id.Dot);
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
            buttonPlus.Click += Button_Click;
            buttonMin.Click += Button_Click;
            buttonDiv.Click += Button_Click;
            buttonMult.Click += Button_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonDelete.Click += Button_Click;
            buttonSign.Click += Button_Click;
            buttonDot.Click += Button_Click;
            buttonEqual.Click += ButtonEqual_Click;


        }

        private void ButtonEqual_Click(object sender, System.EventArgs e)
        {
            double Input1 = 0.0;
            double Input2 = 0.0;
            double Result = 0.0;
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


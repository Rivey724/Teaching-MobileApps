using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using System;
using System.Threading;

namespace GoogleApiExample
{
    [Activity(Label = "GoogleApiExample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public Boolean challenge_start = false;
        string[] PicItems = { "Tree", "Cup", "Car", "Apple", "Bannaa", "Table", "BackPack", "Cat", "Dog", "Onion", "Pan", "Door", "Battery", "Chair",
                                                  "Scissors", "Bed", "Water Bottle", "Refridgerator", "Toilet", "Sink", "Pencil", "Pen", "Bush", "Broom", "Shirt" };

        Android.Graphics.Bitmap bitmap;
        string ChosenItem;
        bool Win;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures() == true)
            {
                FindViewById<Button>(Resource.Id.launchCameraButton).Click += TakePicture;
            }

            //Reference for future: There are 25 items in the array
            //Starts the challenge
            Button Start_Challenge = FindViewById<Button>(Resource.Id.StartGame);
            Start_Challenge.Click += Start_Challenge_Click;

            //Confirms that the pic taken is the one to be used
            Button Confirm = FindViewById<Button>(Resource.Id.Submit);
            Confirm.Click += Confirm_Click;

            //Takes the user back to the camera to take another pic
            Button AnotherPic = FindViewById<Button>(Resource.Id.AnotherPic);
            AnotherPic.Click += TakePicture;

            //End Screen button, returns you to the main screen
            Button GoBack = FindViewById<Button>(Resource.Id.GoBack);
            GoBack.Click += Back_to_Start;
        }

        private void Back_to_Start(object sender, EventArgs e)
        {
            //Takes the user back to the start screen
            SetContentView(Resource.Layout.Main);
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            //Needs to take the user to the final game screen
            //Checks the API against the chosen item to see if the user
            //Took the right picture
            SetContentView(Resource.Layout.GameEnd);

            ImageView Confirm_Pic = FindViewById<ImageView>(Resource.Id.PicView);
            Confirm_Pic.SetImageBitmap(bitmap);
        }
         
        private void Start_Challenge_Click(object sender, EventArgs e)
        {
            //To not cause issues if the button is pressed again
            Random random_num = new Random();
            int random_item = random_num.Next(0, 25);
        
            //Handles starting the game, by starting a timer and grabbing a random item from the PicItems array
            if (challenge_start == false)
            {
                ChosenItem = PicItems[random_item];

                //Timer Section
                TimeState s = new TimeState();

                TimerCallback timer_del = new TimerCallback(CheckStatus);

                //SHOULD start on click
                Timer game_time = new Timer(timer_del, s, 0, 1000);


                //TEST AREA FOR TIMER
                s.timer = game_time;
                //Changes challenge start to true so that clicking the button again
                //Doesn't restart the timer
                challenge_start = true;

                //Declare ID for TimerDisplay and set the timer to it
                TextView Timer_Disp = FindViewById<TextView>(Resource.Id.TimerDisplay);
                Timer_Disp.Text = System.Convert.ToString(game_time);
            }

          

        }

        //Handles timer basics such as starting the counter 
        //at 0 and decreasing it every second
        private void CheckStatus(Object state)
        {
            TimeState t = (TimeState)state;
            t.counter = 60;
            t.counter--;
            if(t.counter == 0)
            {
                //This ends the game as time has ran out. Move to End layout and display that time has ran out
                //TODO: implement layout change and text view text
                //A Button should be made on the final layout to take you back, this button should
                //Handle changing challenge_start to false
                SetContentView(Resource.Id.EndGame_Screen);

                TextView End = FindViewById<TextView>(Resource.Id.EndText);
                End.Text = string.Format("Sorry, time ran out!");
            }
        }

        /// <summary>
        /// Apparently, some android devices do not have a camera.  To guard against this,
        /// we need to make sure that we can take pictures before we actually try to take a picture.
        /// </summary>
        /// <returns></returns>
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities
                (intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }


        private void TakePicture(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }

        // <summary>
        // Called automatically whenever an activity finishes
        // </summary>
        // <param name = "requestCode" ></ param >
        // < param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            SetContentView(Resource.Layout.Confirm_Screen);
            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPictureImageView);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;

            //AC: workaround for not passing actual files
            bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");


            //convert bitmap into stream to be sent to Google API
            string bitmapString = "";
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 0, stream);

                var bytes = stream.ToArray();
                bitmapString = System.Convert.ToBase64String(bytes);
            }

            //credential is stored in "assets" folder
            string credPath = "google_api.json";
            Google.Apis.Auth.OAuth2.GoogleCredential cred;

            //Load credentials into object form
            using (var stream = Assets.Open(credPath))
            {
                cred = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(stream);
            }
            cred = cred.CreateScoped(Google.Apis.Vision.v1.VisionService.Scope.CloudPlatform);

            // By default, the library client will authenticate 
            // using the service account file (created in the Google Developers 
            // Console) specified by the GOOGLE_APPLICATION_CREDENTIALS 
            // environment variable. We are specifying our own credentials via json file.
            var client = new Google.Apis.Vision.v1.VisionService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApplicationName = "subtle-isotope-190917",
                HttpClientInitializer = cred
            });

            //set up request
            var request = new Google.Apis.Vision.v1.Data.AnnotateImageRequest();
            request.Image = new Google.Apis.Vision.v1.Data.Image();
            request.Image.Content = bitmapString;

            //tell google that we want to perform label detection
            request.Features = new List<Google.Apis.Vision.v1.Data.Feature>();
            request.Features.Add(new Google.Apis.Vision.v1.Data.Feature() { Type = "LABEL_DETECTION" });
            var batch = new Google.Apis.Vision.v1.Data.BatchAnnotateImagesRequest();
            batch.Requests = new List<Google.Apis.Vision.v1.Data.AnnotateImageRequest>();
            batch.Requests.Add(request);

            //send request.  Note that I'm calling execute() here, but you might want to use
            //ExecuteAsync instead
            var apiResult = client.Images.Annotate(batch).Execute();
            for(int i = 0; i < 10; i++)
                if (apiResult.Responses[i].LabelAnnotations[i].Description == ChosenItem)
                {
                    //Change Screen to GameEnd Screen
                    SetContentView(Resource.Layout.GameEnd);
                    //Display that you found the correct item
                    TextView End = FindViewById<TextView>(Resource.Id.EndText);
                    End.Text = string.Format("Congratulations, you took a picture of: " + ChosenItem);
                    Win = true;
                }

            if (Win != true)
            {
                //Prompt the user to take another Picture
                TextView Pic_F = FindViewById<TextView>(Resource.Id.Pic_Fail);
                Pic_F.Text = string.Format("Pic does not match, please take another");
            }

            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                bitmap = null;
            }

            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }
    }
    class TimeState
    {
            public int counter = 0;
            public Timer timer;
        //IDEA: Can we cast the timer or counter to a variable and pass it to the screen?
            
    }
}


using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using System;
using Android.Graphics;
using System.IO;

namespace Project2
{
    [Activity(Label = "Project2", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Used to track the file that we're manipulating between functions
        /// </summary>
        public static Java.IO.File _file;
     

        /// <summary>
        /// Used to track the directory that we'll be writing to between functions
        /// </summary>
        public static Java.IO.File _dir;

        //Global bitmap Variables to allow for image manipulation
        public static Bitmap Bitmap;
        public static Bitmap copyBitmap;

        //Another Global Bitmap Variable that handles RemoveLast
        public static Bitmap LastBitmap;
        public static Bitmap OriginalBitmap;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures() == true)
            {
                CreateDirectoryForPictures();
                FindViewById<Button>(Resource.Id.launchCameraButton).Click += TakePicture;
                FindViewById<Button>(Resource.Id.launchGallery).Click += OpenGallery;
            }

        }

        private void OpenGallery(object sender, EventArgs e)
        {
            //This should allow us to open the gallery and select a photo to manipulate
            var ImageIntent = new Intent();
            ImageIntent.SetType("image/*");
            ImageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(ImageIntent, "Select Photo"), 1);
           
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

        /// <summary>
        /// Creates a directory on the phone that we can place our images
        /// </summary>
        private void CreateDirectoryForPictures()
        {
            _dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraExample");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private void TakePicture(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            //android.support.v4.content.FileProvider
            //getUriForFile(getContext(), "com.mydomain.fileprovider", newFile);
            //FileProvider.GetUriForFile

            //The line is a problem line for Android 7+ development
            //intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
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

            //Make image available in the gallery
            /*
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Android.Net.Uri.FromFile(_file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            */
            
            SetContentView(Resource.Layout.Editor);
            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            int height = imageView.Height;
            int width = imageView.Width;

            if(requestCode == 1)
            {

            }
            //AC: workaround for not passing actual files

            Bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
            
            //Android.Graphics.Bitmap bitmap = _file.Path.LoadAndResizeBitmap(width, height);
            copyBitmap = Bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
            OriginalBitmap = Bitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for(int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    //00000000 00000000 00000000 00000000
                    //long mask = (long)0xFF00FFFF;
                    //p = p & (int)mask;
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    c.R = 0;
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            if (Bitmap != null)
            {
                imageView.SetImageBitmap(Bitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                Bitmap = null;
            }

            Button Red_Button = FindViewById<Button>(Resource.Id.Red);
            Button Blue_Button = FindViewById<Button>(Resource.Id.Blue);
            Button Green_Button = FindViewById<Button>(Resource.Id.Green);
            Button Negate_Red = FindViewById<Button>(Resource.Id.NegRed);
            Button Negate_Blue = FindViewById<Button>(Resource.Id.NegBlue);
            Button Negate_Green = FindViewById<Button>(Resource.Id.NegGreen);
            Button High_Contrast = FindViewById<Button>(Resource.Id.HighContrast);
            Button Add_Noise = FindViewById<Button>(Resource.Id.Noise);
            Button GrayScale = FindViewById<Button>(Resource.Id.GrayScale);
            Button RemoveLast = FindViewById<Button>(Resource.Id.RemoveLast);
            Button SaveFinish = FindViewById<Button>(Resource.Id.Finish);
            Button ClearAll = FindViewById<Button>(Resource.Id.ClearPicture);
            Red_Button.Click += Red_Button_Click;
            Blue_Button.Click += Blue_Button_Click;
            Green_Button.Click += Green_Button_Click;
            Negate_Red.Click += Neg_Red_Click;
            Negate_Blue.Click += Neg_Blue_Click;
            Negate_Green.Click += Neg_Green_Click;
            Add_Noise.Click += Add_Noise_Click;
            GrayScale.Click += GrayScale_Click;
            High_Contrast.Click += High_Contrast_Click;
            RemoveLast.Click += Remove_Last_Click;
            SaveFinish.Click += Save_Click;
            ClearAll.Click += Clear_All_Click;

           



             // Dispose of the Java side bitmap.
             System.GC.Collect();
        }

        private void Clear_All_Click(object sender, EventArgs e)
        {
            //This button Clears all image effects by grabbing the original Bitmap
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            imageView.SetImageBitmap(OriginalBitmap);
            copyBitmap = OriginalBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageSave);
            imageView.SetImageBitmap(copyBitmap);

            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, "test.png");
            var stream = new FileStream(filePath, FileMode.Create);
            copyBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
        }

        private void Remove_Last_Click(object sender, EventArgs e)
        {
            //All this button does is Undo the Previous Image Effect
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            
            imageView.SetImageBitmap(LastBitmap);
        }

        private void High_Contrast_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    if(c.R < System.Convert.ToByte(255/2))
                    {
                        c.R = 0;
                    }
                    else
                    {
                        c.R = 255;
                    }
                    if (c.B < System.Convert.ToByte(255 / 2))
                    {
                        c.B = 0;
                    }
                    else
                    {
                        c.B = 255;
                    }
                    if (c.G < System.Convert.ToByte(255 / 2))
                    {
                        c.G = 0;
                    }
                    else
                    {
                        c.G = 255;
                    }
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap); 
        }

        private void GrayScale_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    int d = (c.R + c.B + c.G) / 3;
                    c.R = System.Convert.ToByte(d);
                    c.B = System.Convert.ToByte(d);
                    c.G = System.Convert.ToByte(d);
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Add_Noise_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    Random add_random = new Random();
                    Int32 AddNoise = add_random.Next(-10, 10);
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    int TempRed = c.R;
                    TempRed += AddNoise;
                    if (TempRed < 0)
                    {
                        c.R = 0;
                    }
                    else if (TempRed > 255)
                    {
                        c.R = 255;
                    }
                    else
                    {
                        c.R = Convert.ToByte(TempRed);
                    }

                    int TempBlue = c.B;
                    TempBlue += AddNoise;
                    if (TempBlue < 0)
                    {
                        c.B = 0;
                    }
                    else if (TempBlue> 255)
                    {
                        c.B = 255;
                    }
                    else
                    {
                        c.B = Convert.ToByte(TempBlue);
                    }

                    int TempGreen = c.G;
                    TempGreen += AddNoise;
                    if (TempGreen < 0)
                    {
                        c.G = 0;
                    }
                    else if (TempGreen > 255)
                    {
                        c.G = 255;
                    }
                    else
                    {
                        c.G = Convert.ToByte(TempGreen);
                    }
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Neg_Green_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    int tempGreen = c.G;
                    tempGreen = 255 - tempGreen;
                    c.R = Convert.ToByte(tempGreen);
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Neg_Blue_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    int tempBlue = c.B;
                    tempBlue = 255 - tempBlue;
                    c.B = Convert.ToByte(tempBlue);
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Neg_Red_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    int tempRed = c.R;
                    tempRed = 255 - tempRed;
                    c.R = Convert.ToByte(tempRed);
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Green_Button_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    c.G = 0;
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Blue_Button_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    c.B = 0;
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }

        private void Red_Button_Click(object sender, EventArgs e)
        {
            LastBitmap = copyBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true); ;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.EditImage);
            for (int i = 0; i < copyBitmap.Width; i++)
            {
                for (int j = 0; j < copyBitmap.Height; j++)
                {
                    int p = copyBitmap.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    c.R = 0;
                    copyBitmap.SetPixel(i, j, c);
                }
            }
            imageView.SetImageBitmap(copyBitmap);
        }
    }
}


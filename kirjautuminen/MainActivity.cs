﻿using System.Net;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Net;

namespace kirjautuminen
{
    
    [Activity(MainLauncher = true, Icon = "@drawable/pix")]
    public class MainActivity : Activity
    {
        private Button _mBtnSignIn;
        private Button _mBtnSignUp;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle); //This will Hide the title Bar
            SetContentView(Resource.Layout.Main);

            _mBtnSignUp = FindViewById<Button>(Resource.Id.BtnSignUp); //Clicking signup
            _mBtnSignUp.Click += (sender, args) =>
            {
                //Pull up sign up dialog
                var transaction = FragmentManager.BeginTransaction();
                var signUpDialog = new dialogSignUp();
                signUpDialog.Show(transaction, "dialog fragment");
            };
            
            _mBtnSignIn = FindViewById<Button>(Resource.Id.BtnSignIn); //Clicking signin
            _mBtnSignIn.Click += (sender, args) =>
            {             
                var usernameField = FindViewById<EditText>(Resource.Id.userName);
                var passwordField = FindViewById<EditText>(Resource.Id.password);
                var username = usernameField.Text;
                var password = passwordField.Text;

                if (IsValid.Password(password, password))
                {
                    if (IsValid.Username(username))
                    {
                        var success = ApiService.Login(username, password);
                        Toast.MakeText(this, "You are in!!!", ToastLength.Long);
                    }
                }
                else
                {
                    Toast.MakeText(this, "Something went wrong", ToastLength.Long);
                }
            };
        }
       
    }
}
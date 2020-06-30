using System;
using Android.Hardware.Biometrics;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using Java.Lang;
using Java.Security;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(BioAuth.Droid.BiometricProvider))]
namespace BioAuth.Droid
{
    public class BiometricProvider : IBiometricProvider
    {
        public void Authenticate(IBioAuthCompleted bioAuthCompleted)
        {
            var context = CrossCurrentActivity.Current.AppContext;
            var biometricManager = BiometricManager.From(context);
            switch (biometricManager.CanAuthenticate())
            {
                case BiometricManager.BiometricSuccess:
                    //Log.d("MY_APP_TAG", "App can authenticate using biometrics.");
                    ShowBiometicPrompt(bioAuthCompleted);
                    break;
                case BiometricManager.BiometricErrorNoHardware:
                    bioAuthCompleted.OnCompleted(BioAuthStatus.ENROLL_BiometricErrorNoHardware);
                    break;
                case BiometricManager.BiometricErrorHwUnavailable:
                    bioAuthCompleted.OnCompleted(BioAuthStatus.ENROLL_BiometricErrorHwUnavailable);
                    break;
                case BiometricManager.BiometricErrorNoneEnrolled:
                    bioAuthCompleted.OnCompleted(BioAuthStatus.ENROLL_BiometricErrorNoneEnrolled);
                    break;
            }

            //MessagingCenter.Send(this, "BioAuth", "Message from Android");
            //bioAuthCompleted.OnCompleted(BioAuthStatus.SUCCESS);
        }

        private void ShowBiometicPrompt(IBioAuthCompleted bioAuthCompleted)
        {
            var activity = (FragmentActivity)CrossCurrentActivity.Current.Activity;
            var executor = ContextCompat.GetMainExecutor(activity);

            var callback = new BiometricAuthenticationCallback
            {
                Success = (AndroidX.Biometric.BiometricPrompt.AuthenticationResult result) => {
                    try
                    {
                        bioAuthCompleted.OnCompleted(BioAuthStatus.SUCCESS);
                    }
                    catch (SignatureException)
                    {
                        throw new RuntimeException();
                    }
                },
                Failed = () => {
                    // TODO: Show error.
                    bioAuthCompleted.OnCompleted(BioAuthStatus.FAILED);
                },
                Help = (BiometricAcquiredStatus helpCode, ICharSequence helpString) => {
                    // TODO: What do we do here?
                }
            };
            
            //Create prompt info
            var promptInfo = new AndroidX.Biometric.BiometricPrompt.PromptInfo.Builder()
            .SetTitle("Biometric login for my app")
            .SetSubtitle("Log in using your biometric credential")
            .SetNegativeButtonText("Use account password")
            .Build();

            //Create prompt
            var biometricPrompt = new AndroidX.Biometric.BiometricPrompt(activity, executor, callback);

            //call Authenticate
            biometricPrompt.Authenticate(promptInfo);
        }

        class BiometricAuthenticationCallback : AndroidX.Biometric.BiometricPrompt.AuthenticationCallback
        {
            public Action<AndroidX.Biometric.BiometricPrompt.AuthenticationResult> Success;
            public Action Failed;
            public Action<BiometricAcquiredStatus, ICharSequence> Help;

            public override void OnAuthenticationSucceeded(AndroidX.Biometric.BiometricPrompt.AuthenticationResult result)
            {
                base.OnAuthenticationSucceeded(result);
                Success(result);
            }

            public override void OnAuthenticationFailed()
            {
                base.OnAuthenticationFailed();
                Failed();
            }

            //public override void ([GeneratedEnum] BiometricAcquiredStatus helpCode, ICharSequence helpString)
            //{
            //    base.OnAuthenticationHelp(helpCode, helpString);
            //    Help(helpCode, helpString);
            //}
        }
    }
}

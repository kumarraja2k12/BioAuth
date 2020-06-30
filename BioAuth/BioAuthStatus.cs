using System;
namespace BioAuth
{
    public enum BioAuthStatus
    {
        SUCCESS,
        FAILED,

        ENROLL_BiometricSuccess,
        ENROLL_BiometricErrorNoHardware,
        ENROLL_BiometricErrorHwUnavailable,
        ENROLL_BiometricErrorNoneEnrolled
    }
}

namespace BioAuth
{
    public interface IBiometricProvider
    {
        void Authenticate(IBioAuthCompleted bioAuthCompleted);
    }
}

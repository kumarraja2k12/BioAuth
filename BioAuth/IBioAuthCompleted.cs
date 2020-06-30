using System;
namespace BioAuth
{
    public interface IBioAuthCompleted
    {
        void OnCompleted(BioAuthStatus status);
    }
}

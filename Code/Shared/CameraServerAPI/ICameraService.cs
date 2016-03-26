using System.ServiceModel;

namespace RaspicamServer
{
    [ServiceContract]
    public interface ICameraService
    {
        [OperationContract]
        void SetupCamera(RaspicamParameters parameters);

        [OperationContract]
        byte[] CaptureImage();
    }
}
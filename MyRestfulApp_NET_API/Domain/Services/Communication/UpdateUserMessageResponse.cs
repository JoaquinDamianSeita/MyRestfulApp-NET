using MyRestfulApp_NET_API.Resources;

namespace MyRestfulApp_NET_API.Domain.Services.Communication;

public class UpdateUserMessageResponse : BasicMessageResponse
{
    public UserResource? User { get; private set; }
    public UpdateUserMessageResponse(bool success, string message, int errorCode, UserResource user)
      : base(success, message, errorCode)
    {
        User = user;
    }
}

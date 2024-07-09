using Microservices.Web.Models;

namespace Microservices.Web.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDTO> SendAsync(RequestDTO requestDTO,bool withBearer=true);
    }
}

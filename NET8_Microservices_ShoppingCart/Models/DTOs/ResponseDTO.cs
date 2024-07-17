namespace NET8_Microservices_ShoppingCartAPI.Models.DTOs
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
    }
}

namespace NET8_Microservices_Udemy.Services.CouponAPI.Models.DTOs
{
	public class ResponseDTO
	{
		public object? Result { get; set; }
		public bool IsSuccess { get; set; } = true;
		public string Message { get; set; } = "";
    }
}

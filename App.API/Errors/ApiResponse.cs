namespace App.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int _code)
        {
            Code = _code;
            Message = GetMessage(_code);
        }
        public ApiResponse(int _code, string _message)
        {
            Code = _code;
            Message = _message;
        }

        public int Code { get; set; }
        public string Message { get; set; }

        private string GetMessage(int code)
            => code switch
            {
                200 => "success",
                201 => "created",
                400 => "bad request",
                401 => "unauthorized access",
                404 => "not found resources",
                _ => "internal server error"
            };
    }
}

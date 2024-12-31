namespace Com.Scm
{
    public class OidcResponse
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public virtual bool IsSuccess()
        {
            return Success;
        }

        public virtual string GetMessage()
        {
            return Message;
        }

        public virtual void SetSuccess()
        {
            Success = true;
        }

        public virtual void SetSuccess(int code)
        {
            Success = true;
            Code = code;
        }

        public virtual void SetSuccess(string message)
        {
            Success = true;
            Message = message;
        }

        public virtual void SetSuccess(int code, string message)
        {
            Success = true;
            Code = code;
            Message = message;
        }

        public virtual void SetFailure(int code)
        {
            Success = false;
            Code = code;
        }

        public virtual void SetFailure(string message)
        {
            Success = false;
            Message = message;
        }

        public virtual void SetFailure(int code, string message)
        {
            Success = false;
            Code = code;
            Message = message;
        }
    }

    public class OidcDataResponse<T> : OidcResponse
    {
        public T Data { get; set; }

        public void SetSuccess(T data)
        {
            Success = true;
            Code = 0;
            Message = "";
            Data = data;
        }
    }
}

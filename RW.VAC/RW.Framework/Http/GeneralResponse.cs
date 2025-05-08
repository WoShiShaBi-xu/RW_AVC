using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW.Framework.Extensions;

namespace RW.Framework.Http
{
    public class GeneralResponse
    {
        public GeneralResponse(int code)
        {
            Code = code;
            Message = string.Empty;
        }

        public GeneralResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public static string Success() => new GeneralResponse(0, string.Empty).ToJson();

        public static string Error(int code, string message) => new GeneralResponse(code, message).ToJson();
    }

    public class GeneralResponse<T> : GeneralResponse
    {
        public T Data { get; }

        public GeneralResponse(int code, T data) : base(code)
        {
            Data = data;
        }

        public GeneralResponse(int code, string message, T data) : base(code, message)
        {
            Data = data;
        }

        public static string Success(T data) => new GeneralResponse<T>(0, data).ToJson();
    }
}

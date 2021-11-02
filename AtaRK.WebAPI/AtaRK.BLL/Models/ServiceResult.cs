using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Models
{
    public class ServiceResult
    {
        public bool IsSuccessful { get; set; }

        public static ServiceResult Instance(bool status = false)
        {
            return new ServiceResult()
            {
                IsSuccessful = status
            };
        }

        public static implicit operator ServiceResult(bool status)
        {
            return new ServiceResult()
            {
                IsSuccessful = status
            };
        }
    }

    public class ServiceResult<TResult> : ServiceResult
    {
        public TResult Result { get; set; }

        public static new ServiceResult<TResult> Instance(bool status = false)
        {
            return new ServiceResult<TResult>()
            {
                IsSuccessful = status
            };
        }

        public static ServiceResult<TResult> Instance(TResult result)
        {
            return new ServiceResult<TResult>()
            {
                IsSuccessful = true,
                Result = result
            };
        }

        public static implicit operator ServiceResult<TResult>(TResult result)
        {
            return new ServiceResult<TResult>()
            {
                IsSuccessful = true,
                Result = result
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Domain
{
    public class Result <T> 
    {
        public T Value { get;}
        public bool IsSuccess { get;}
        public string Error { get;}

        protected Result(T value, bool success, string error)
        {
            Value = value;
            IsSuccess = success;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null);
        public static Result<T> Failure(string error) => new(default,false, error);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GTBack.Core.DTO
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }
        public List<string>? Errors { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }

        public static CustomResponseDto<T> Success(int statusCode,T data)
        {

            return new CustomResponseDto<T> { StatusCode = statusCode, Data = data ,Errors=null};
        }
        public static CustomResponseDto<T> Error(int statusCode, List<string> errors)
        {

            return new CustomResponseDto<T> { StatusCode = statusCode,Errors=errors};
        }
        public static CustomResponseDto<T> Success(int statusCode)
        {

            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Error(int statusCode, string error)
        {

            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }

    }
}

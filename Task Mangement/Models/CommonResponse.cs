﻿using System.Net;

namespace Task_Mangement.Models
{
    public class CommonResponse
    {
        public bool Status { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string Message { get; set; } = "Something went wrong! Please try again.";
        public dynamic Data { get; set; } = null;
    }
}

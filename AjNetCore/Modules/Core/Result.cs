using AjNetCore.Modules.Core.Filters;
using Humanizer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AjNetCore.Modules.Core
{
    public class Result
    {
        public Result()
        {
            Success = true;
        }

        public int? Id { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public MessageType MessageType { get; set; } = MessageType.Success;

        public IDictionary<string, string> Errors { get; set; } = new ConcurrentDictionary<string, string>();

        private bool? _isRedirect;
        public bool IsRedirect
        {
            get => _isRedirect ?? !string.IsNullOrEmpty(Redirect);
            set => _isRedirect = value;
        }

        public string Redirect { get; set; }

        public bool ClearForm { get; set; }

        public Paging Paging { get; set; } = new Paging();

        #region Helpers

        public Result SetError(string message)
        {
            Success = false;
            Message = message;
            MessageType = MessageType.Error;

            return this;
        }

        public Result SetBlankRedirect()
        {
            IsRedirect = true;

            return this;
        }

        public Result SetRedirect(string url)
        {
            Redirect = url;

            return this;
        }

        public Result SetSuccess(string message = null)
        {
            Success = true;

            if (!string.IsNullOrEmpty(message))
                Message = message;

            MessageType = MessageType.Success;

            return this;
        }

        public Result SetSuccess(string message, params object[] args)
        {
            return SetSuccess(string.Format(message, args));
        }

        public Result Clear()
        {
            ClearForm = true;

            return this;
        }

        public Result SetPaging(int page, int size, int total)
        {
            Paging.Size = size == 0 ? 10 : size;
            Paging.Page = page == 0 ? 1 : page;
            Paging.Total = total;

            return this;
        }

        public Result SetPaging(BaseFilterDto dto, int total)
        {
            SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, total);

            Paging.SortColumn = (dto?.SortColumn ?? "createdAt").Camelize();
            Paging.SortType = (dto?.SortType ?? "desc").ToLower();

            return this;
        }

        #endregion

        public Result SetData(dynamic data)
        {
            Data = data;

            return this;
        }

        public dynamic Data { get; set; }

        [OnSerializing]
        internal void OnSerializedMethod(StreamingContext context)
        {
            if (Errors.All(w => w.Key != "")) return;

            Message = Errors.First().Value;
            Errors.Clear();
        }
    }

    public enum MessageType
    {
        Error = 0,
        Success = 1,
        Info = 2,
        Warning = 3
    }

    public class Paging
    {
        public int Page { get; set; }
        public int LastPage
        {
            get
            {
                if (Page == 0) return 0;

                var lastPage = Total / (decimal)Size;

                if (lastPage == 0)
                    lastPage = 1;

                return (int)Math.Ceiling(lastPage);
            }
        }
        public int Size { get; set; }
        public int Total { get; set; }
        public string SortColumn { get; set; }
        public string SortType { get; set; }
    }
}
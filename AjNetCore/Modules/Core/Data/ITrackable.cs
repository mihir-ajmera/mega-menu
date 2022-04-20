using System;

namespace AjNetCore.Modules.Core.Data
{
    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
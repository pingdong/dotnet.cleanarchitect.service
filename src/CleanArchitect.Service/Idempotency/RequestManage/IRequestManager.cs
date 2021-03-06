﻿using System.Threading.Tasks;

namespace PingDong.CleanArchitect.Service
{
    public interface IRequestManager<in TId>
    {
        Task EnsureNotExistsAsync(TId id);

        Task CreateRequestRecordAsync(TId id);
    }
}

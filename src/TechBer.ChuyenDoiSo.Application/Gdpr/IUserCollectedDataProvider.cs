﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}

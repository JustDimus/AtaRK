﻿using AtaRK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Interfaces
{
    public interface IGroupService
    {
        Task<ServiceResult> CreateGroupAsync()
    }
}

// FRM.Core.Interfaces.Services/ILayoutService.cs
using FRM.Core.DTOs;
using System;

namespace FRM.Core.Interfaces.Services
{
    public interface ILayoutService
    {
        // Метод должен быть СИНХРОННЫМ
        LayoutUserDto GetLayoutUserData(Guid userId);
    }
}
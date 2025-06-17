// FRM.Core.Interfaces.Services/ILikeService.cs
using FRM.Core.Entities;
using System.Threading.Tasks;
using System;

public interface ILikeService
{
    Task<int> ToggleLikeAsync(Guid commentId, Guid userId);
}


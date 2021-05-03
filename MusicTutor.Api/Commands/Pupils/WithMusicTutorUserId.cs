using System;
using MediatR;
using MusicTutor.Api.Contracts.Pupils;
using MusicTutor.Core.Models;

namespace MusicTutor.Api.Commands.Pupils
{
    public interface IResponseContract { }

    public class WithMusicTutorUserId<TRequest, TResponse> : IRequest<TResponse>
    {
        public Guid MusicTutorUserId { get; init; }

        public TRequest Request { get; init; }

        public WithMusicTutorUserId(Guid musicTutorUserId, TRequest request)
        {
            MusicTutorUserId = musicTutorUserId;
            Request = request;
        }
    }

}
using System;
using System.Collections.Generic;
using MapsterMapper;
using MusicTutor.Api.UnitTests.Mapping;
using MusicTutor.Api.UnitTests.Utils;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Api.UnitTests.Handlers.Pupils
{

    public abstract class PupilHandlerUnitTest
    {
        protected readonly IMusicTutorDbContext _dbContext;
        protected readonly Instrument _instrument;
        protected readonly Instrument _secondInstrument;
        protected readonly Pupil _pupil;
        protected readonly IMapper _mapper;
        protected readonly MusicTutorUser _currentUser;

        public PupilHandlerUnitTest()
        {
            _currentUser = new MusicTutorUser();
            _instrument = Instrument.CreateInstrument("TEST");
            _secondInstrument = Instrument.CreateInstrument("TEST2");
            var instruments = new List<Instrument>();
            instruments.Add(_instrument);
            instruments.Add(_secondInstrument);
            _pupil = Pupil.CreatePupil("PupilName", 14M, DateTime.Now, 7, instruments, "ContactName", "ContactEmail", "ContactPhoneNumber");
            _pupil.AssignToMusicTutorUser(_currentUser.Id);
            _pupil.AddCompletedLesson(DateTime.Now.AddDays(-1), 30, _pupil.CurrentLessonRate);
            _dbContext = MockDbContextBuilder.Init().WithInstruments(_instrument).WithPupils(_pupil).Build();
            _mapper = MappingBuilder.Init().Build();
        }
    }
}
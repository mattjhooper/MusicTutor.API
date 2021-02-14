using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using MusicTutor.Api.Behaviors;
using MusicTutor.Api.Commands.Pupils;
using MusicTutor.Api.Validators.Pupils;
using NSubstitute;
using Xunit;

namespace MusicTutor.Api.UnitTests.Behaviors
{
    public class ValidationBehaviorUnitTests
    {
        private readonly ValidationBehavior<IPipelineValidate,int> _sut;
        private readonly RequestHandlerDelegate<int> _next;
        public ValidationBehaviorUnitTests()
        {
            
            _next = Substitute.For<RequestHandlerDelegate<int>>();

            var validators = Substitute.For<IEnumerable<IValidator<IPipelineValidate>>>();
            
            _sut = new ValidationBehavior<IPipelineValidate,int>(validators);
        }

        [Fact]
        public async Task WhenNoValidators_NextCalled()
        {
            //Given
            var request = new DeletePupilInstrumentLink(Guid.NewGuid(), Guid.NewGuid());
            
            //When
            var response = await _sut.Handle(request, new CancellationToken(), _next);

            //Then
            _next.Received(1);
        }

        [Fact]
        public void ValidateFailure_ThrowsError()
        {
            //Given
            var dbValidator = Substitute.For<IDbValidator>();
            dbValidator.PupilInstrumentCanBeRemovedAsync(Arg.Any<DeletePupilInstrumentLink>(), default).Returns(false);
            var validator = new DeletePupilInstrumentLinkValidator(dbValidator);
            var validators = new List<DeletePupilInstrumentLinkValidator>
            {
                validator
            };
            var validationBehavior = new ValidationBehavior<DeletePupilInstrumentLink, int>(validators);
            var request = new DeletePupilInstrumentLink(Guid.NewGuid(), Guid.NewGuid());

            //When
            validationBehavior.Invoking(y => y.Handle(request, new CancellationToken(), _next))
                .Should().Throw<FluentValidation.ValidationException>();

            //Then
            _next.DidNotReceive();
        }

    }    
}

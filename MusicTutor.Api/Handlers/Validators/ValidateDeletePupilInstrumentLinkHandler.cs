using MediatR;
using MusicTutor.Api.Contracts.Errors;
using MusicTutor.Api.Validators.Pupils;
using MusicTutor.Api.Commands.Validators;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace MusicTutor.Api.EFCore.Handlers.Validators
{
    public record ValidateDeletePupilInstrumentLinkHandler(DeletePupilInstrumentLinkValidator Validator) : IRequestHandler<ValidateCommand, ErrorResponse>
    {        
        public async Task<ErrorResponse> Handle(ValidateCommand request, CancellationToken cancellationToken)
        {
            var validationResponse = await Validator.ValidateAsync(request.Command);
            if (!validationResponse.IsValid)
            {
                var errorList = new List<ErrorModel>();
                foreach(var failure in validationResponse.Errors)
                {
                    errorList.Add(new ErrorModel(failure.PropertyName, failure.ErrorMessage));
                }

                var errorResponse = new ErrorResponse
                {
                    Errors = errorList
                };

                return errorResponse;
            }

            return null;
        }
    }
}
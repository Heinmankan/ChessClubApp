using ChessClub.API.Models;
using ChessClub.Database.Models;
using ChessClub.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChessClub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ChessController : ControllerBase
    {
        private readonly ILogger<ChessController> _logger;
        private readonly IChessClubService _chessClubService;

        public ChessController(ILogger<ChessController> logger, IChessClubService chessClubService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chessClubService = chessClubService ?? throw new ArgumentNullException(nameof(chessClubService));
        }

        [HttpPost("Members")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetMemberResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembers(GetMemberRequest request)
        {
            try
            {
                IEnumerable<Member> result;

                if (request.PageNumber > 0 && request.PageSize > 0)
                {
                    result = await Task.Run(() => _chessClubService.GetMembers(request.PageNumber, request.PageSize));
                }
                else
                {
                    result = await Task.Run(() => _chessClubService.GetMembers());
                }

                return Ok(new GetMemberResponse
                {
                    Members = result.Select(m => new MemberDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Surname = m.Surname,
                        Email = m.Email,
                        Birthday = m.Birthday,
                        GamesPlayed = m.GamesPlayed,
                        CurrentRank = m.CurrentRank
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }

        [HttpPost("AddMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddMemberResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            try
            {
                var result = await Task.Run(() => _chessClubService.AddMember(request.Name, request.Surname, request.Email, request.Birthday));

                return Ok(new AddMemberResponse
                {
                    Id = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }

        [HttpPost("UpdateMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(UpdateMemberRequest request)
        {
            try
            {
                var result = await Task.Run(() => _chessClubService.UpdateMember(request.Id, request.Name, request.Surname, request.Email, request.Birthday));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }

        [HttpPost("DeleteMember")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember(DeleteMemberRequest request)
        {
            try
            {
                var result = await Task.Run(() => _chessClubService.DeleteMember(request.Id));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }
    }
}

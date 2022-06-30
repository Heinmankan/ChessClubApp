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

        [HttpGet("Members")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetMembersResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembers([FromQuery]int pageNumber, [FromQuery]int pageSize)
        {
            try
            {
                IEnumerable<Member> result;

                if (pageNumber > 0 && pageSize > 0)
                {
                    result = await Task.Run(() => _chessClubService.GetMembers(pageNumber, pageSize));
                }
                else
                {
                    result = await Task.Run(() => _chessClubService.GetMembers());
                }

                var memberList = result.Select(m => new MemberDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Surname = m.Surname,
                    Email = m.Email,
                    Birthday = m.Birthday,
                    GamesPlayed = m.GamesPlayed,
                    CurrentRank = m.CurrentRank
                });

                return Ok(new GetMembersResponse
                {
                    Members = memberList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }

        [HttpGet("Members/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            try
            {
                Member result;

                result = await Task.Run(() => _chessClubService.GetMemberById(id));

                return Ok(new MemberDTO
                {
                    Id = result.Id,
                    Name = result.Name,
                    Surname = result.Surname,
                    Email = result.Email,
                    Birthday = result.Birthday,
                    GamesPlayed = result.GamesPlayed,
                    CurrentRank = result.CurrentRank
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

        [HttpPost("AddResult")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddResult(AddResultRequest request)
        {
            try
            {
                var result = await Task.Run(() => _chessClubService.AddResult(request.Player1, request.Player2, request.Winner));

                return Ok(new AddResultResponse
                {
                    IsSuccess = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {type} - {message}", ex.GetType(), ex.Message);

                throw;
            }
        }
    }
}

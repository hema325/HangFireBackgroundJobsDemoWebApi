using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFireBackgroundJobsDemoWebApi.Controllers
{
    [ApiController]
    [Route("hangfire")]
    public class HangfireController : ControllerBase
    {
        private readonly ILogger<HangfireController> _logger;

        public HangfireController(ILogger<HangfireController> logger)
        {
            _logger = logger;
        }

        [HttpGet("enqueue")]
        public IActionResult Enqueue()
        {
            var jobId = BackgroundJob.Enqueue(() => SendMessage());
            return Ok(jobId);
        }

        [HttpGet("requeue")]
        public IActionResult Requeue(string jobId)
        {
            var isRequeued = BackgroundJob.Requeue(jobId);
            return Ok(isRequeued);
        }

        [HttpGet("schedule")]
        public IActionResult Schedule()
        {
            var jobId = BackgroundJob.Schedule(() => SendMessage(), TimeSpan.FromMinutes(1));
            return Ok(jobId);
        }

        [HttpGet("continueJobWith")]
        public IActionResult ContinueJobWith(string parentId)
        {
            var isRequeued = BackgroundJob.ContinueJobWith(parentId, () => SendMessage());
            return Ok(isRequeued);
        }

        [HttpGet("addOrUpdate")]
        public IActionResult AddOrUpdate()
        {
            RecurringJob.AddOrUpdate(() => SendMessage(), Cron.Minutely);
            return NoContent();
        }

        [HttpDelete("{jobId}")]
        public IActionResult DeleteJob(string jobId)
        {
            var isDeleted = BackgroundJob.Delete(jobId);
            return Ok(isDeleted);
        }

        [NonAction]
        public void SendMessage()
        {
            _logger.LogInformation("Message sent at {dateTime}", DateTime.UtcNow);
        }
    }
}

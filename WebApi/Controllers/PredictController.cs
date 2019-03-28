using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Akka.Actor;
using WebApi.Domain.Entities;
using WebApi.Domain.Messages;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictController : ControllerBase
    {
        private readonly IActorRef _predictActorPool;

        public PredictController(IActorRef predictActorPool)
        {
            _predictActorPool = predictActorPool;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] IrisData observation)
        {
            IrisPrediction prediction = await _predictActorPool
                .Ask<IrisPrediction>(new Predict<IrisData> {
                    Observation = observation
                });
            return prediction.PredictedLabel;
        }
    }
}
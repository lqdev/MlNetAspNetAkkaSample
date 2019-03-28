using System;
using System.IO;
using Akka;
using Akka.Actor;
using Microsoft.ML;
using WebApi.Domain.Entities;
using WebApi.Domain.Messages;

namespace WebApi.Actors
{
    public class PredictActor : UntypedActor
    {
        private MLContext _mlContext;
        private ITransformer _mlModel;
        private PredictionEngine<IrisData,IrisPrediction> _predictionEngine;
        
        public PredictActor(string modelPath)
        {
            _mlContext = new MLContext();

            using(var fs = File.OpenRead(modelPath))
            {
                _mlModel = _mlContext.Model.Load(fs);
            }

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<IrisData,IrisPrediction>(_mlModel);
        }
        protected override void OnReceive(object message)
        {
            message
                .Match()
                .With<Predict<IrisData>>(msg => HandleMessage(msg.Observation));
        }

        private void HandleMessage(IrisData observation)
        {
            IrisPrediction prediction = _predictionEngine.Predict(observation);
            Sender.Tell(prediction,Self);
        }
    }
}
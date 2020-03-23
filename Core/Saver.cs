using Microsoft.EntityFrameworkCore;
using ArticyProjectJsonParser.Database.Context;
using ArticyProjectJsonParser.Database.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace ArticyProjectJsonParser.Core
{
    public class Saver
    {
        public void Save(ParsingResult result)
        {
            var jDoc = result.Result;
            var jModelsArray = jDoc.RootElement.GetProperty("Packages").GetProperty("Models");

            foreach (var jModel in jModelsArray.EnumerateArray())
            {
                var modelDbo = new ModelDbo
                {
                    Type = jModel.GetProperty("Type").GetString(),
                    TechnicalName = jModel.GetProperty("TechnicalName").GetString(),
                    Id = jModel.GetProperty("Id").GetString(),
                    Parent = jModel.GetProperty("Parent").GetString(),
                    DisplayName = jModel.GetProperty("DisplayName").GetString(),
                    Color = jModel.GetProperty("Color").GetString(),
                    Text = jModel.GetProperty("Text").GetString(),
                    ExternalId = jModel.GetProperty("ExternalId").GetString(),
                    ShortId = jModel.GetProperty("ShortId").GetInt32()
                };

                var pinDbos = new List<PinDbo>();
                var connectionDbos = new List<ConnectionDbo>();

                var jInputPins = jModel.GetProperty("InputPins");
                var inputPinsAndConnections = GetPinsAndConnections(jInputPins);
                var inputPinDbos = inputPinsAndConnections.Pins;
                var inputConnectionDbos = inputPinsAndConnections.Connections;
                pinDbos.AddRange(inputPinDbos);
                connectionDbos.AddRange(inputConnectionDbos);

                var jOuputPins = jModel.GetProperty("OutputPins");
                var outputPinsAndConnections = GetPinsAndConnections(jOuputPins);
                var outputPinDbos = outputPinsAndConnections.Pins;
                var outputConnectionDbos = outputPinsAndConnections.Connections;
                pinDbos.AddRange(outputPinDbos);
                connectionDbos.AddRange(outputConnectionDbos);

                using (var context = new ArticyProjectDbContext())
                {
                    context.Add(modelDbo);
                    context.AddRange(pinDbos);
                    context.AddRange(connectionDbos);

                    context.SaveChanges();
                }
            }  
        }

        private (List<PinDbo> Pins, List<ConnectionDbo> Connections) GetPinsAndConnections(JsonElement pinsElement)
        {
            var pinDbos = new List<PinDbo>();
            var connectionDbos = new List<ConnectionDbo>();

            foreach (var jPin in pinsElement.EnumerateArray())
            {
                var pinDbo = new PinDbo
                {
                    Text = jPin.GetProperty("Text").GetString(),
                    Id = jPin.GetProperty("Id").GetString(),
                    Owner = jPin.GetProperty("Owner").GetString()
                };

                pinDbos.Add(pinDbo);

                var jConnections = jPin.GetProperty("Connections");

                foreach (var jConnection in jConnections.EnumerateArray())
                {
                    var connectionDbo = new ConnectionDbo
                    {
                        Color = jConnection.GetProperty("Color").GetString(),
                        Label = jConnection.GetProperty("Label").GetString(),
                        Source = jConnection.GetProperty("Source").GetString(),
                        SourcePin = jConnection.GetProperty("SourcePin").GetString(),
                        TargetPin = jConnection.GetProperty("TaretPin").GetString(),
                        Target = jConnection.GetProperty("Target").GetString()
                    };

                    connectionDbos.Add(connectionDbo);
                }
            }

            return (pinDbos, connectionDbos);
        }
    }
}
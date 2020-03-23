using Microsoft.EntityFrameworkCore;
using ArticyProjectJsonParser.Database.Context;
using ArticyProjectJsonParser.Database.Models;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Drawing;
using static System.Text.Json.JsonElement;

namespace ArticyProjectJsonParser.Core
{
    public class Saver
    {
        public string DatabasePath { get; set; }

        public Saver(string databasePath)
        {
            DatabasePath = databasePath;
        }
        public void Save(ParsingResult result)
        {
            var jDoc = result.Result;
            var jPackagesArray = jDoc.RootElement.GetProperty("Packages");
            foreach (var jPackage in jPackagesArray.EnumerateArray())
            {
                var jModelsArray = jPackage.GetProperty("Models");

                foreach (var jModel in jModelsArray.EnumerateArray())
                {
                    var type = jModel.GetProperty("Type").GetString();

                    var jProperties = jModel.GetProperty("Properties");

                    var technicalName = jProperties.GetProperty("TechnicalName").GetString();
                    var id = jProperties.GetProperty("Id").GetString();
                    var parent = jProperties.GetProperty("Parent").GetString();
                    var displayNameSuccess = (jProperties.TryGetProperty("DisplayName", out var jDisplayName));

                    string displayName = null;
                    if (displayNameSuccess)
                    {
                        displayName = jDisplayName.GetString();
                    }

                    var colorSuccess = jProperties.TryGetProperty("Color", out var jColor);

                    string color = null;
                    if (colorSuccess)
                    {
                        color = GetColor(jColor);
                    }

                    var textSuccess = jProperties.TryGetProperty("Text", out var jText);

                    string text = null; 
                    if (textSuccess)
                    {
                        text = jText.GetString();
                    }

                    var externalId = jProperties.GetProperty("ExternalId").GetString();
                    var shortId = jProperties.GetProperty("ShortId").GetInt64();

                    var modelDbo = new ModelDbo
                    {
                        Type = type,
                        TechnicalName = technicalName,
                        Id = id,
                        Parent = parent,
                        DisplayName = displayName,
                        Color = color,
                        Text = text,
                        ExternalId = externalId,
                        ShortId = shortId
                    };

                    var pinDbos = new List<PinDbo>();
                    var connectionDbos = new List<ConnectionDbo>();

                    var inputPinsSuccess = jProperties.TryGetProperty("InputPins", out var jInputPins);
                    (List<PinDbo> Pins, List<ConnectionDbo> Connections) inputPinsAndConnections = (null, null);
                    if (inputPinsSuccess)
                    {
                        inputPinsAndConnections = GetPinsAndConnections(jInputPins, type: "Input");
                    
                        var inputPinDbos = inputPinsAndConnections.Pins;
                        var inputConnectionDbos = inputPinsAndConnections.Connections;
                        pinDbos.AddRange(inputPinDbos);
                        connectionDbos.AddRange(inputConnectionDbos);
                    }

                    var outputPinsSuccess = jProperties.TryGetProperty("OutputPins", out var jOutputPins);
                    (List<PinDbo> Pins, List<ConnectionDbo> Connections) outputPinsAndConnections = (null, null);
                    if (inputPinsSuccess)
                    {
                        outputPinsAndConnections = GetPinsAndConnections(jOutputPins, type: "Output");

                        var outputPinDbos = outputPinsAndConnections.Pins;
                        var outputConnectionDbos = outputPinsAndConnections.Connections;
                        pinDbos.AddRange(outputPinDbos);
                        connectionDbos.AddRange(outputConnectionDbos);
                    }

                    using (var context = new ArticyProjectDbContext(DatabasePath))
                    {
                        context.Add(modelDbo);
                        context.AddRange(pinDbos);
                        context.AddRange(connectionDbos);

                        context.SaveChanges();
                    }
                }  
            }
        }

        private (List<PinDbo> Pins, List<ConnectionDbo> Connections) GetPinsAndConnections(JsonElement pinsElement, string type)
        {
            var pinDbos = new List<PinDbo>();
            var connectionDbos = new List<ConnectionDbo>();

            foreach (var jPin in pinsElement.EnumerateArray())
            {
                var pinDbo = new PinDbo
                {
                    Type = type,
                    Text = jPin.GetProperty("Text").GetString(),
                    Id = jPin.GetProperty("Id").GetString(),
                    Owner = jPin.GetProperty("Owner").GetString()
                };

                pinDbos.Add(pinDbo);

                var success = jPin.TryGetProperty("Connections", out var jConnections);

                if (!success)
                {
                    continue;
                }

                foreach (var jConnection in jConnections.EnumerateArray())
                {
                    var connectionDbo = new ConnectionDbo
                    {
                        Color = GetColor(jConnection.GetProperty("Color")),
                        Label = jConnection.GetProperty("Label").GetString(),
                        SourcePin = jPin.GetProperty("Id").GetString(),
                        Source = jPin.GetProperty("Owner").GetString(),
                        TargetPin = jConnection.GetProperty("TargetPin").GetString(),
                        Target = jConnection.GetProperty("Target").GetString()
                    };

                    connectionDbos.Add(connectionDbo);
                }
            }

            return (pinDbos, connectionDbos);
        }

        private string GetColor(JsonElement jColor)
        {
            var r = (int)(jColor.GetProperty("r").GetDouble() * byte.MaxValue);
            var g = (int)(jColor.GetProperty("g").GetDouble() * byte.MaxValue);
            var b = (int)(jColor.GetProperty("b").GetDouble() * byte.MaxValue);

            var hex =  "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");

            return hex;
        }
    }
}
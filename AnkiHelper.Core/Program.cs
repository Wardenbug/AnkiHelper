using AnkiHelper.Core;
using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAnkiHelperCore(builder.Configuration);

using var app = builder.Build();

var ankiHelper = app.Services.GetService<IAnkiHelper>();
